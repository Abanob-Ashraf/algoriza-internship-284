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
    public class AppointmentConfig : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> entity)
        {
            entity.ToTable("appointment", "patient");

            entity.Property(e => e.ResevationDate)
                .IsRequired(true)
                .HasColumnType("datetime2");

            entity.Property(e => e.Status)
                .IsRequired(true)
                .HasDefaultValue(Status.pending);

            entity.HasOne(e => e.PatientIdNavigation)
                .WithMany(c => c.Appointments)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appoinment_patient");

            entity.HasOne(e => e.DoctorIdNavigation)
                .WithMany(c => c.Appointments)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appoinment_doctor");

            entity.HasOne(e => e.DiscountIdNavigation)
                .WithMany(c => c.Appointments)
                .HasForeignKey(e => e.DiscountId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appoinment_discount");
        }
    }
}
