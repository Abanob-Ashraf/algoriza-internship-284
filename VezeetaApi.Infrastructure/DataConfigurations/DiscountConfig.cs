using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Infrastructure.DataConfigurations
{
    public class DiscountConfig : BaseConfig<Discount, int>
    {
        public override void Configure(EntityTypeBuilder<Discount> entity)
        {
            entity
                .ToTable("discount");

            entity
                .Property(e => e.DiscountCode)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(false);

            entity
                .Property(e => e.NumOfCompletedRequests)
                .IsRequired(true);

            entity
                .Property(e => e.DiscountType)
                .IsRequired(true);

            entity
                .Property(e => e.DiscountValue)
                .IsRequired(true);

            entity
                .HasIndex(e => e.DiscountCode)
                .IsUnique(true);

            entity
                .HasData(new Discount()
                {
                    Id = 1,
                    DiscountCode = "Test10",
                    NumOfCompletedRequests = 5,
                    DiscountType = DiscountType.Percentage,
                    DiscountValue = 10
                });
            
            entity
                .HasData(new Discount()
                {
                    Id = 2,
                    DiscountCode = "Test02",
                    NumOfCompletedRequests = 2,
                    DiscountType = DiscountType.Value,
                    DiscountValue = 50
                });

            base.Configure(entity);
        }
    }
}
