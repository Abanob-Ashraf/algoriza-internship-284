using System.ComponentModel.DataAnnotations;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Domain.Dtos
{
    public class RegisterDTO
    {
        public string? Image { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "start with 010 | 011 | 012 | 015 and max 11 Diget")]
        [MaxLength(11)]
        public string PhoneNumber { get; set; } = null!;

        public string? UserName
        {
            get { return $"{Email}"; }
        }

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        [Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;

        public Gender  Gender { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BirthDate { get; set; }


    }
}
