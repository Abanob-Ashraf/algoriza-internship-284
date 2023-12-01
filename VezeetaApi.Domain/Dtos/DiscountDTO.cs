using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Domain.Dtos
{
    public class DiscountDTO
    {
        [JsonIgnore]
        public int Id { get; set; }

        public int NumOfCompletedRequests { get; set; }

        public DiscountType DiscountType { get; set; }

        public int DiscountValue { get; set; }
    }
}
