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
    internal class DoctorScheduleConfig : IEntityTypeConfiguration<DoctorSchedule>
    {
        public void Configure(EntityTypeBuilder<DoctorSchedule> entity)
        {
            entity.ToTable("doctorSchedule", "doctor");

            entity.Property(e => e.Amount)
                .IsRequired(true)
                .HasColumnType("decimal");

            entity.Property(e => e.ScheduleDay)
                .IsRequired(true);

            entity.Property(e => e.ScheduleTime)
                .IsRequired(true);

            entity.HasIndex(e => new
            {
                e.ScheduleDay,
                e.ScheduleTime,
                e.DoctorId
            }).IsUnique(true);

            entity.HasOne(e => e.DoctorIdNavigation)
                .WithMany(c => c.DoctorSchedules)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_doctorSchedule_doctor");

            entity.HasData(new DoctorSchedule()
            {
                Id = 1,
                Amount = 70.00,
                ScheduleDay = Day.Monday,
                ScheduleTime = new TimeSpan(15, 30, 0),
                DoctorId = 1
            });
        }
    }
}
