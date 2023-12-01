using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Domain.Dtos
{
    public class PatientDTO
    {
        [JsonIgnore]
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
