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

        public string? PatientPhone { get; set; }

        public string? PatientEmail { get; set; }

        public string? PatientPassword { get; set; }
    }
}
