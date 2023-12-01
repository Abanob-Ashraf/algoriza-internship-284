using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Dtos;

namespace VezeetaApi.Domain.Models
{
    public class DoctorSchedule : BaseEntity<int>
    {
        public double Amount { get; set; }

        public Day ScheduleDay { get; set; }

        public TimeSpan ScheduleTime { get; set; }

        public int DoctorId { get; set; }

        public virtual Doctor? DoctorIdNavigation { get; set; }
    }

    public enum Day
    {
        Saturday = 0,
        Sunday = 1,
        Monday = 2,
        Tuesday = 3,
        Wednesday = 4,
        Thursday = 5,
        Friday = 6,
    }
}
