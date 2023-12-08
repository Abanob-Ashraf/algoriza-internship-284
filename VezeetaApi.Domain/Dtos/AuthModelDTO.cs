using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VezeetaApi.Domain.Dtos
{
    public class AuthModelDTO
    {
        public string? Message { get; set; }

        public bool IsAuthenticated { get; set; }

        public string? UserName 
        {
            get { return $"{Email}"; }
        }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public List<string>? Roles { get; set; }

        public string? Token { get; set; }

        public DateTime ExpiredOn { get; set; }
        
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
