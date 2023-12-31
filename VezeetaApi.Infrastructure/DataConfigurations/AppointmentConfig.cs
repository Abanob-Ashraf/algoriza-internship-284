﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Infrastructure.DataConfigurations
{
    public class AppointmentConfig : BaseConfig<Appointment, int>
    {
        public override void Configure(EntityTypeBuilder<Appointment> entity)
        {
            entity
                .ToTable("appointment", "patient");

            entity
                .Property(e => e.ResevationDate)
                .IsRequired(true)
                .HasColumnType("datetime2");

            entity
                .Property(e => e.Status)
                .IsRequired(true)
                .HasDefaultValue(Status.pending);

            entity
                .HasOne(e => e.PatientIdNavigation)
                .WithMany(c => c.Appointments)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appoinment_patient");

            entity
                .HasOne(e => e.DoctorIdNavigation)
                .WithMany(c => c.Appointments)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appoinment_doctor");

            entity
                .HasOne(e => e.DiscountIdNavigation)
                .WithMany(c => c.Appointments)
                .HasForeignKey(e => e.DiscountId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appoinment_discount");

            entity
                .HasData(new Appointment()
                {
                    Id = 1,
                    ResevationDate = DateTime.Now,
                    Status = Status.pending,
                    PatientId = 1,
                    DoctorId = 1,
                    DiscountId = 1,
                });

            base.Configure(entity);
        }
    }
}
