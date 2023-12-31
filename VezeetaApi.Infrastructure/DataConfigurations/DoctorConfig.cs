﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Infrastructure.DataConfigurations
{
    public class DoctorConfig : BaseConfig<Doctor, int>
    {
        public override void Configure(EntityTypeBuilder<Doctor> entity)
        {

            entity
                .ToTable("doctor", "doctor");

            entity
                .Property(e => e.DocImage)
                .IsRequired(true)
                .HasColumnType("varchar(max)")
                .IsUnicode(false);

            entity
                .Property(e => e.DocFirstName)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(false);

            entity
                .Property(e => e.DocLastName)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(false);

            entity
                .Property(e => e.DocBirthDate)
                .IsRequired(true)
                .HasColumnType("datetime2");

            entity
                .Property(e => e.DocGender)
                .IsRequired(true);

            entity
                .Property(e => e.DocPhone)
                .HasMaxLength(11)
                .IsRequired(true)
                .IsUnicode(false);

            entity
                .Property(e => e.DocEmail)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(false);

            entity
                .Property(e=> e.DocPassword)
                .HasColumnType("varchar(max)")
                .IsRequired(false)
                .IsUnicode(false);

            entity
                .HasIndex(e => e.DocEmail)
                .IsUnique(true);

            entity
                .HasIndex(e => e.DocPhone)
                .IsUnique(true);

            entity
                .HasOne(e => e.SpecializationIdNavigation)
                .WithMany(e => e.Doctors)
                .HasForeignKey(e => e.SpecializationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_doctor_specialization");

            entity
                .HasData(new Doctor()
                {
                    Id = 1,
                    DocImage = "Image.Jpg",
                    DocFirstName = "Test",
                    DocLastName = "Doctor",
                    DocBirthDate = new DateTime(1998, 10, 31),
                    DocPhone = "01234567892",
                    DocEmail = "testdoctor@vezeeta.org",
                    DocPassword = "Admin$123",
                    SpecializationId = 1,
                });

            base.Configure(entity);
        }
    }
}
