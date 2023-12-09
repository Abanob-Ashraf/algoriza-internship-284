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
        readonly JWT Jwt;
        readonly IUnitOfWork UnitOfWork;

        public AuthRepository(UserManager<AppUser> userManager, IMapper mapper, IOptions<JWT> jwt, IUnitOfWork unitOfWork)
        {
            UserManager = userManager;
            Mapper = mapper;
            Jwt = jwt.Value;
            UnitOfWork = unitOfWork;
        }

        public async Task<AuthDTO> RegistrationAsync(RegisterDTO registerDto)
        {
            var phones = UserManager.Users.Select(c => c.PhoneNumber).ToList();
            if (phones.Contains(registerDto.PhoneNumber))
                return new AuthDTO { Message = "PhoneNumber is already registered!" };

            if (await UserManager.FindByEmailAsync(registerDto.Email) is not null)
                return new AuthDTO { Message = "Email is already registered!" };

            var user = Mapper.Map<AppUser>(registerDto);

            var result = await UserManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new AuthDTO { Message = errors };
            }

            PatientDTO patientDTO = new PatientDTO()
            {
                PatientImage = registerDto.Image,
                PatientFirstName = user.FirstName,
                PatientLastName = user.LastName,
                PatientBirthDate = registerDto.BirthDate,
                PatientGender = registerDto.Gender,
                PatientPhone = user.PhoneNumber,
                PatientEmail = user.Email,
                PatientPassword = user.PasswordHash,
            };

            var newPatient = Mapper.Map<Patient>(patientDTO);
            await UnitOfWork.GetRepository<Patient>().AddAsync(newPatient);
            await UnitOfWork.SaveChangesAsync();

            await UserManager.AddToRoleAsync(user, "Patient");
            var token = await CreateJwtToken(user, newPatient.Id);

            return new AuthDTO
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

        public async Task<AuthDTO> LoginAsync(LoginDTO LoginDto)
        {
            var authDto = new AuthDTO();
            int? id = null;
            var user = await UserManager.FindByEmailAsync(LoginDto.Email);
            if (user is null || !await UserManager.CheckPasswordAsync(user, LoginDto.Password))
            {
                authDto.Message = "Email or Password is incorrect!";
                return authDto;
            }

            var roles = await UserManager.GetRolesAsync(user);

            if (roles.Contains("Patient")) 
            {
                var patient = await UnitOfWork.GetRepository<Patient>().FindAsync(c => c.PatientEmail == user.Email);
                id = patient.Id;
            }
            else if (roles.Contains("Doctor"))
            {
                var doctor = await UnitOfWork.GetRepository<Doctor>().FindAsync(c => c.DocEmail == user.Email);
                id = doctor.Id;

            }
            else if (roles.Contains("Admin"))
            {
                id = null;
            }

           var token = await CreateJwtToken(user, id);
           
            authDto.IsAuthenticated = true;
            authDto.PhoneNumber = user.PhoneNumber;
            authDto.Roles = roles.ToList();
            authDto.Email = user.Email;
            authDto.Token = new JwtSecurityTokenHandler().WriteToken(token);
            authDto.ExpiredOn = token.ValidTo;

            if (user.RefreshTokens.Any(c => c.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.SingleOrDefault(c => c.IsActive);
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

        public async Task<AuthDTO> RefreshTokenAsync(string token)
        {
            var authModel = new AuthDTO();

            var user = await UserManager.Users.SingleOrDefaultAsync(c => c.RefreshTokens.Any(b => b.Token == token));

            if (user is null)
            {
                authModel.Message = "Invalid token";
                return authModel;
            }

            var refreshToken = user.RefreshTokens.Single(c => c.Token == token);

            if (!refreshToken.IsActive)
            {
                authModel.Message = "Inactive token";
                return authModel;
            }

            refreshToken.UpdatedDate = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await UserManager.UpdateAsync(user);

            var jwtToken = await CreateJwtToken(user, null);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.Email = user.Email;
            var roles = await UserManager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiredOn;

            return authModel;
        }


        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await UserManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user is null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshToken.IsActive)
                return false;

            refreshToken.UpdatedDate = DateTime.UtcNow;

            await UserManager.UpdateAsync(user);
            return true;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(AppUser user, int? id)
        {
            var userClaims = await UserManager.GetClaimsAsync(user);
            var roles = await UserManager.GetRolesAsync(user);
            var roleClaim = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaim.Add(new Claim(ClaimTypes.Role, role));
            }

            var FullName = $"{user.FirstName} {user.LastName}";
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            };
            if (id.HasValue)
            {
                claims.Add(new Claim("DbUserId", id.Value.ToString()));
            }
            claims = claims.Union(userClaims).Union(roleClaim).ToList();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt.Key));
            var signInCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Jwt.ValidIssuer,
                audience: Jwt.ValidAudiance,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Jwt.DurationInMinutes),
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
                ExpiredOn = DateTime.UtcNow.AddMinutes(10),
                CreatedDate = DateTime.UtcNow,
            };
        }
    }
}
