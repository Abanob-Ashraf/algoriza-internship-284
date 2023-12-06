using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Infrastructure.DataConfigurations
{
    public class DoctorSpecializionConfig : IEntityTypeConfiguration<DoctorSpecializion>
    {
        public void Configure(EntityTypeBuilder<DoctorSpecializion> entity)
        {
            entity.ToTable("doctorSpecializion", "doctor");

            entity.Property(e => e.SpecializationName)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(false);

            entity.HasIndex(e => e.SpecializationName).IsUnique(true);
        }
    }
}
