using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Dtos;

namespace VezeetaApi.Domain.Models
{
    public class Discount : BaseEntity<int>
    {
        public string DiscountCode { get; set; }

        public int NumOfCompletedRequests { get; set; }

        public DiscountType DiscountType { get; set; }

        public int DiscountValue { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }

    public enum DiscountType
    {
        Percentage = 0,
        Value = 1,
    }
}
