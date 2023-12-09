using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Dtos;

namespace VezeetaApi.Domain.Services
{
    public interface IAuthService
    {
        Task<AuthDTO> RegistrationAsync(RegisterDTO dto);
        Task<AuthDTO> LoginAsync(LoginDTO dto);
        Task<AuthDTO> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}
