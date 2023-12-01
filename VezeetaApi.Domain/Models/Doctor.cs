using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Dtos;

namespace VezeetaApi.Domain.Models
{
    public class Doctor : BaseEntity<int>
    {
        public string DocImage { get; set; }

        [NotMapped]
        public string DoctorFullName
        {
            get { return $"{DocFirstName} {DocLastName}"; }
        }

        public string DocFirstName { get; set; }

        public string DocLastName { get; set; }

        public DateTime DocBirthDate { get; set; }

        public Gender DocGender { get; set; }

        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "start with 010 | 011 | 012 | 015 and max 11 Diget")]
        [MaxLength(11)]
        public string DocPhone { get; set; }

        public string DocEmail { get; set; }

        public string? DocPassword { get; set; }

        public int SpecializationId { get; set; }

        public virtual DoctorSpecializion? SpecializationIdNavigation { get; set; }

        public virtual ICollection<Appointment>? Appointments { get; set; } = new HashSet<Appointment>();

        public virtual ICollection<DoctorSchedule>? DoctorSchedules { get; set; } = new HashSet<DoctorSchedule>();

    }
    public enum Gender
    {
        Male = 0,
        Female = 1
    }
}
