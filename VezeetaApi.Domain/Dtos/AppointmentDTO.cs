using VezeetaApi.Domain.Models;

namespace VezeetaApi.Domain.Dtos
{
    public class AppointmentDTO
    {
        public int Id { get; set; }

        public DateTime ResevationDate { get; set; }

        public Status Status { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public int? DiscountId { get; set; }
    }
}
