using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Infrastructure.DataConfigurations
{
    public class PatientConfig : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> entity)
        {
            entity.ToTable("patient", "patient");

            entity.Property(e => e.PatientImage)
                .IsRequired(false)
                .HasColumnType("varchar(max)")
                .IsUnicode(false);

            entity.Property(e => e.PatientFirstName)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(false);

            entity.Property(e => e.PatientLastName)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(false);

            entity.Property(e => e.PatientBirthDate)
                .IsRequired(true)
                .HasColumnType("datetime2");

            entity.Property(e => e.PatientGender)
                .IsRequired(true);

            entity.Property(e => e.PatientPhone)
                .HasMaxLength(11)
                .IsRequired(true)
                .IsUnicode(false);

            entity.Property(e => e.PatientEmail)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(false);

            entity.Property(e => e.PatientPassword)
                .HasColumnType("varchar(max)")
                .IsRequired(false)
                .IsUnicode(false);

            entity.HasIndex(e => e.PatientEmail).IsUnique(true);

            entity.HasIndex(e => e.PatientPhone).IsUnique(true);
        }
    }
}
