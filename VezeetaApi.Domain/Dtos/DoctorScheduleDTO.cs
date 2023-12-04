using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Domain.Dtos
{
    public class DoctorScheduleDTO
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        public Day ScheduleDay { get; set; }

        public TimeSpan ScheduleTime { get; set; }

        public int DoctorId { get; set; }
    }
}
