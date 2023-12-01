using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Domain.Dtos
{
    public class DoctorDTO
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string DocFirstName { get; set; }

        public string DocLastName { get; set; }

        public DateTime DocBirthDate { get; set; }

        public Gender DocGender { get; set; }

        public string DocPhone { get; set; }

        public string DocEmail { get; set; }

        public string? DocPassword { get; set; }

        public int SpecializationId { get; set; }
    }
}
