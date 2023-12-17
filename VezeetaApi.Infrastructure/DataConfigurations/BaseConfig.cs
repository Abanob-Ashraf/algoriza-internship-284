using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Dtos;

namespace VezeetaApi.Infrastructure.DataConfigurations
{
    public class BaseConfig<TEntity, T> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity<T>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> entity)
        {
            entity
                .HasKey(e => e.Id);

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
