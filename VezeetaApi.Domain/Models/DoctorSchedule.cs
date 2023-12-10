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
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
    }
}
