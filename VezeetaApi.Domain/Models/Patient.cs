using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Dtos;

namespace VezeetaApi.Domain.Models
{
    public class Patient : BaseEntity<int>
    {
        public string? PatientImage { get; set; }

        [NotMapped]
        public string PatientFullName
        {
            get { return $"{PatientFirstName} {PatientLastName}"; }
        }

        public string PatientFirstName { get; set; }

        public string PatientLastName { get; set; }

        //[CustomMaxDate]
        public DateTime? PatientBirthDate { get; set; }

        public Gender? PatientGender { get; set; }

        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "start with 010 | 011 | 012 | 015 and max 11 Diget")]
        public string? PatientPhone { get; set; }

        public string? PatientEmail { get; set; }

        public string? PatientPassword { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }
}
