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

        public DateTime DocBirthDate { get; set; }

        public Gender DocGender { get; set; }

        public string DocPhone { get; set; } = null!;

        public string DocEmail { get; set; } = null!;

        [JsonIgnore]
        public string? DocPassword { get; set; }

        public int SpecializationId { get; set; }
    }
}
