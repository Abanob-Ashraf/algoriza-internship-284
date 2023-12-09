using VezeetaApi.Domain.Dtos;

namespace VezeetaApi.Domain.Models
{
    public class DoctorSpecializion : BaseEntity<int>
    {
        public string SpecializationName { get; set; } = null!;

        public virtual ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();
    }
}
