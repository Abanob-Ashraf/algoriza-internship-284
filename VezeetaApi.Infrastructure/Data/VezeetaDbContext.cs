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
                    entityTypeBuilder.Property("CreatedDate").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");
                    entityTypeBuilder.Property("UpdatedDate").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");
                    entityTypeBuilder.Property("IsDeleted").IsRequired(true);
                }
            }
            new DoctorSpecializionConfig().Configure(modelBuilder.Entity<DoctorSpecializion>());

            new DoctorConfig().Configure(modelBuilder.Entity<Doctor>());

            new DoctorScheduleConfig().Configure(modelBuilder.Entity<DoctorSchedule>());

            new DiscountConfig().Configure(modelBuilder.Entity<Discount>());

            new PatientConfig().Configure(modelBuilder.Entity<Patient>());

            new AppointmentConfig().Configure(modelBuilder.Entity<Appointment>());
        }

        //public override int SaveChanges()
        //{
        //    var entries = ChangeTracker
        //        .Entries()
        //        .Where(e => e.Entity.GetType().BaseType != null
        //            && e.Entity.GetType().BaseType.IsGenericType
        //            && e.Entity.GetType().BaseType.GetGenericTypeDefinition() == typeof(BaseEntity<>)
        //            && (e.State == EntityState.Added || e.State == EntityState.Modified));

        //    foreach (var entityEntry in entries)
        //    {
        //        // Get the actual type of the entity
        //        var entityType = entityEntry.Entity.GetType();
        //        // Get the PropertyInfo of UpdatedDate
        //        var updatedDateProperty = entityType.GetProperty("UpdatedDate");
        //        // Set the value of UpdatedDate
        //        updatedDateProperty.SetValue(entityEntry.Entity, DateTime.Now);

        //        if (entityEntry.State == EntityState.Added)
        //        {
        //            // Get the PropertyInfo of CreatedDate
        //            var createdDateProperty = entityType.GetProperty("CreatedDate");
        //            // Set the value of CreatedDate
        //            createdDateProperty.SetValue(entityEntry.Entity, DateTime.Now);
        //        }
        //    }
        //    return base.SaveChanges();
        //}

        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var entries = ChangeTracker
        //        .Entries()
        //        .Where(e => e.Entity.GetType().BaseType != null
        //            && e.Entity.GetType().BaseType.IsGenericType
        //            && e.Entity.GetType().BaseType.GetGenericTypeDefinition() == typeof(BaseEntity<>)
        //            && (e.State == EntityState.Added || e.State == EntityState.Modified));

        //    foreach (var entityEntry in entries)
        //    {
        //        // Get the actual type of the entity
        //        var entityType = entityEntry.Entity.GetType();
        //        // Get the PropertyInfo of UpdatedDate
        //        var updatedDateProperty = entityType.GetProperty("UpdatedDate");
        //        // Set the value of UpdatedDate
        //        updatedDateProperty.SetValue(entityEntry.Entity, DateTime.Now);

        //        if (entityEntry.State == EntityState.Added)
        //        {
        //            // Get the PropertyInfo of CreatedDate
        //            var createdDateProperty = entityType.GetProperty("CreatedDate");
        //            // Set the value of CreatedDate
        //            createdDateProperty.SetValue(entityEntry.Entity, DateTime.Now);
        //        }
        //    }
        //    return base.SaveChangesAsync(cancellationToken);
        //}
    }
}
