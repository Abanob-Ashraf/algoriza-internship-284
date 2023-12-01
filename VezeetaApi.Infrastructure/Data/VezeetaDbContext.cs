using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Models;
using VezeetaApi.Infrastructure.DataConfigurations;
using VezeetaApi.Infrastructure.Repositories;

namespace VezeetaApi.Infrastructure.Data
{
    public class VezeetaDbContext : IdentityDbContext<AppUser>
    {
        public VezeetaDbContext(DbContextOptions<VezeetaDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<DoctorSpecializion> DoctorSpecializions { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }

        public DbSet<Discount> Discounts { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var baseType = entityType.ClrType.BaseType;
                if (baseType != null && baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(BaseEntity<>))
                {
                    var entityTypeBuilder = modelBuilder.Entity(entityType.ClrType);
                    entityTypeBuilder.HasKey("Id");
                    entityTypeBuilder.Property("CreatedDate").HasDefaultValueSql("GETDATE()");
                    entityTypeBuilder.Property("UpdatedDate").HasDefaultValueSql("GETDATE()").ValueGeneratedOnUpdate();
                    entityTypeBuilder.Property("IsDeleted").IsRequired(true).HasDefaultValueSql("0");
                }
            }
            new DoctorSpecializionConfig().Configure(modelBuilder.Entity<DoctorSpecializion>());

            new DoctorConfig().Configure(modelBuilder.Entity<Doctor>());

            new DoctorScheduleConfig().Configure(modelBuilder.Entity<DoctorSchedule>());

            new DiscountConfig().Configure(modelBuilder.Entity<Discount>());

            new PatientConfig().Configure(modelBuilder.Entity<Patient>());

            new AppointmentConfig().Configure(modelBuilder.Entity<Appointment>());

        }
    }
}
