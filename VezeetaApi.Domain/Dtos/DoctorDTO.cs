using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Domain.Dtos
{
    public class DoctorDTO
    {
        public int Id { get; set; }

        public string DocImage { get; set; } = null!;

        public string DocFirstName { get; set; } = null!;

        public string DocLastName { get; set; } = null!;

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DocBirthDate { get; set; }

        public Gender DocGender { get; set; }

        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "start with 010 | 011 | 012 | 015 and max 11 Diget")]
        [MaxLength(11)]
        public string DocPhone { get; set; } = null!;

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string DocEmail { get; set; } = null!;

        [JsonIgnore]
        public string? DocPassword { get; set; }

        public int SpecializationId { get; set; }
    }
}
