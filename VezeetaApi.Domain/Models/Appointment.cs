using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Dtos;

namespace VezeetaApi.Domain.Models
{
    public class Appointment : BaseEntity<int>
    {
        public DateTime ResevationDate { get; set; }

        public Status Status { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public int? DiscountId { get; set; }

        public virtual Patient? PatientIdNavigation { get; set; }

        public virtual Doctor? DoctorIdNavigation { get; set; }

        public virtual Discount? DiscountIdNavigation { get; set; }
    }

    public enum Status
    {
        pending = 0,
        cancelled = 1,
        completed = 2,
    }
}
