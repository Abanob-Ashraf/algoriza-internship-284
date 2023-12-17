using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Infrastructure.DataConfigurations
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> entity)
        {
            entity
                .ToTable("refreshToken", "token");
            
            entity
                .HasKey(e => new
                {
                    e.Id,
                    e.AppUserId
                });

            entity
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity
                .Property(e => e.CreatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");

            entity
                .Property(e => e.UpdatedDate)
                .HasColumnType("datetime2");

            entity
                .Property(e => e.IsActive)
                .IsRequired(true);
        }
    }
}
