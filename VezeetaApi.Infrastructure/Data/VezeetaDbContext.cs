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

        public DbSet<RefreshToken> RefreshTokens { get; set; }

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
                    entityTypeBuilder.Property("UpdatedDate").HasColumnType("datetime2");
                    entityTypeBuilder.Property("IsActive").IsRequired(true);
                }
            }

            new DoctorSpecializionConfig().Configure(modelBuilder.Entity<DoctorSpecializion>());

            new DoctorConfig().Configure(modelBuilder.Entity<Doctor>());

            new DoctorScheduleConfig().Configure(modelBuilder.Entity<DoctorSchedule>());

            new DiscountConfig().Configure(modelBuilder.Entity<Discount>());

            new PatientConfig().Configure(modelBuilder.Entity<Patient>());

            new AppointmentConfig().Configure(modelBuilder.Entity<Appointment>());

            new RefreshTokenConfig().Configure(modelBuilder.Entity<RefreshToken>());
        }

        public static bool IsSubclassOfRawGeneric(Type genericBase, Type derivedType)
        {
            Type currentType = derivedType;

            while (currentType != null && currentType != typeof(object))
            {
                Type currentGenericType = currentType.IsGenericType ? currentType.GetGenericTypeDefinition() : currentType;
                if (genericBase == currentGenericType)
                {
                    return true;
                }
                currentType = currentType.BaseType;
            }
            return false;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
               .Entries()
               .Where(e => IsSubclassOfRawGeneric(typeof(BaseEntity<>), e.Entity.GetType())
                   && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entityType = entityEntry.Entity.GetType();

                if (entityEntry.State == EntityState.Added)
                {
                    var createdDateProperty = entityType.GetProperty("CreatedDate");

                    createdDateProperty.SetValue(entityEntry.Entity, DateTime.UtcNow);
                }
                else
                {
                    var updatedDateProperty = entityType.GetProperty("UpdatedDate");

                    updatedDateProperty.SetValue(entityEntry.Entity, DateTime.UtcNow);
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
