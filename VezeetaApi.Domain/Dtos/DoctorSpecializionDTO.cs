using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VezeetaApi.Domain.Dtos
{
    public class DoctorSpecializionDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string SpecializationName { get; set; } = null!;
    }
}
