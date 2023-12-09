using VezeetaApi.Domain.Models;

namespace VezeetaApi.Domain.Dtos
{
    public class DiscountDTO
    {
        public int Id { get; set; }

        public string DiscountCode { get; set; } = null!;

        public int NumOfCompletedRequests { get; set; }

        public DiscountType DiscountType { get; set; }

        public int DiscountValue { get; set; }
    }
}
