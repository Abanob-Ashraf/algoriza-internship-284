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
        public int Id { get; set; }

        public string DocImage { get; set; } = null!;

        public string DocFirstName { get; set; } = null!;

        public string DocLastName { get; set; } = null!;

        public DateTime DocBirthDate { get; set; }

        public Gender DocGender { get; set; }

        public string DocPhone { get; set; } = null!;

        public string DocEmail { get; set; } = null!;

        public string DocPassword { get; set; } = null!;

        public int SpecializationId { get; set; }
    }
}
