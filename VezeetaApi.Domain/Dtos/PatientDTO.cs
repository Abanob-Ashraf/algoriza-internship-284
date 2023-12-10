using System.ComponentModel.DataAnnotations;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Domain.Dtos
{
    public class PatientDTO
    {
        public int Id { get; set; }

        public string? PatientImage { get; set; }

        public string PatientFirstName { get; set; } = null!;

        public string PatientLastName { get; set; } = null!;

        public DateTime? PatientBirthDate { get; set; }

        public Gender? PatientGender { get; set; }

        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "start with 010 | 011 | 012 | 015 and max 11 Diget")]
        [MaxLength(11)]
        public string? PatientPhone { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string? PatientEmail { get; set; }

        public string? PatientPassword { get; set; }
    }
}
