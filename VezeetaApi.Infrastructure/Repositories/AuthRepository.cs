using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Helpers;
using VezeetaApi.Domain.Models;
using VezeetaApi.Domain.Services;

namespace VezeetaApi.Infrastructure.Repositories
{
    public class AuthRepository : IAuthService
    {
        readonly UserManager<AppUser> UserManager;
        readonly IMapper Mapper;
        readonly RoleManager<IdentityRole> RoleManager;
        readonly JWT Jwt;
        readonly IUnitOfWork UnitOfWork;

        public AuthRepository(UserManager<AppUser> userManager, IMapper mapper, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            UserManager = userManager;
            Mapper = mapper;
            Jwt = jwt.Value;
            RoleManager = roleManager;
            UnitOfWork = unitOfWork;
        }

        public async Task<AuthModelDTO> RegistrationAsync(RegisterDTO registerDto)
        {
            var phones = UserManager.Users.Select(c => c.PhoneNumber).ToList();
            if (phones.Contains(registerDto.PhoneNumber))
                return new AuthModelDTO { Message = "PhoneNumber is already registered!" };

            if (await UserManager.FindByEmailAsync(registerDto.Email) is not null)
                return new AuthModelDTO { Message = "Email is already registered!" };

            var user = Mapper.Map<AppUser>(registerDto);

            var result = await UserManager.CreateAsync(user, registerDto.Password);

            Patient patient = new Patient()
            {
                PatientFirstName = user.FirstName,
                PatientLastName = user.LastName,
                PatientPhone = user.PhoneNumber,
                PatientEmail = user.Email,
                PatientPassword = user.PasswordHash,
            };

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new AuthModelDTO { Message = errors };
            }

            await UserManager.AddToRoleAsync(user, "Patient");
            var token = await CreateJwtToken(user);
            await UnitOfWork.GetRepository<Patient>().AddAsync(patient);
            return new AuthModelDTO
            {
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiredOn = token.ValidTo,
                IsAuthenticated = true,
                Message = "Registration Successed",
                Roles = new List<string>() { "Patient" }
            };
        }

        public async Task<AuthModelDTO> LoginAsync(LoginDTO LoginDto)
        {
            var authDto = new AuthModelDTO();
            var user = await UserManager.FindByEmailAsync(LoginDto.Email);
            if (user is null || !await UserManager.CheckPasswordAsync(user, LoginDto.Password))
            {
                authDto.Message = "Email or Password is incorrect!";
                return authDto;
            }

            var token = await CreateJwtToken(user);
            var roles = await UserManager.GetRolesAsync(user);

            authDto.IsAuthenticated = true;
            authDto.PhoneNumber = user.PhoneNumber;
            authDto.Roles = roles.ToList();
            authDto.Email = user.Email;
            authDto.Token = new JwtSecurityTokenHandler().WriteToken(token);
            authDto.ExpiredOn = token.ValidTo;

            if (user.RefreshTokens.Any(r => r.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.SingleOrDefault(r => r.IsActive);
                authDto.RefreshToken = activeRefreshToken.Token;
                authDto.RefreshTokenExpiration = activeRefreshToken.ExpiredOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authDto.RefreshToken = refreshToken.Token;
                authDto.RefreshTokenExpiration = refreshToken.ExpiredOn;
                user.RefreshTokens.Add(refreshToken);
                await UserManager.UpdateAsync(user);
            }
            return authDto;
        }

        public async Task<string> AddRoleAsync(RoleDTO model)
        {
            var user = await UserManager.FindByIdAsync(model.UserId);
            if (user is null || await RoleManager.RoleExistsAsync(model.Role))
                return "Invalid user ID or Role";

            if (await UserManager.IsInRoleAsync(user, model.Role))
                return "User already assigned to this role";

            var result = await UserManager.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "Something went worng";
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await UserManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user is null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.Now;

            await UserManager.UpdateAsync(user);
            return true;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
        {
            var userClaims = await UserManager.GetClaimsAsync(user);
            var roles = await UserManager.GetRolesAsync(user);
            var roleClaim = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaim.Add(new Claim(ClaimTypes.Role, role));
            }

            var FullName = $"{user.FirstName} {user.LastName}";
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaim);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt.Key));
            var signInCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Jwt.ValidIssuer,
                audience: Jwt.ValidAudiance,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Jwt.DurationInMinutes),
                signingCredentials: signInCredentials
                );
            return token;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var RandomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(RandomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumber),
                ExpiredOn = DateTime.Now.AddDays(10),
                CreatedOn = DateTime.Now,
            };

        }
    }
}
