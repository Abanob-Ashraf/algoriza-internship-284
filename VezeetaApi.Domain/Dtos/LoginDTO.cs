using System.ComponentModel.DataAnnotations;

namespace VezeetaApi.Domain.Dtos
{
    public class LoginDTO
    {
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
