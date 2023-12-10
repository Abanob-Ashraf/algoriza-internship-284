using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VezeetaApi.Domain.Dtos
{
    public class AuthDTO
    {
        public string? Message { get; set; }

        public bool IsAuthenticated { get; set; }

        public string? UserName 
        {
            get { return $"{Email}"; }
        }

        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "start with 010 | 011 | 012 | 015 and max 11 Diget")]
        [MaxLength(11)]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        public List<string>? Roles { get; set; }

        public string? Token { get; set; }

        public DateTime ExpiredOn { get; set; }
        
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
