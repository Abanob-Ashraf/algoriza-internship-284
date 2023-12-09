using System.ComponentModel.DataAnnotations;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Domain.Dtos
{
    public class RegisterDTO
    {
        public string? Image { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public string? UserName
        {
            get { return $"{Email}"; }
        }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        [Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;

        public Gender  Gender { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BirthDate { get; set; }


    }
}
