using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Infrastructure.DataConfigurations
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> entity)
        {

            entity.HasKey(e => new
            {
                e.Id,
                e.AppUserId
            });

            entity.ToTable("refreshToken", "token");


        }
    }
}
