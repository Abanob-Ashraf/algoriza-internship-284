using Microsoft.EntityFrameworkCore;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Interfaces;
using VezeetaApi.Infrastructure.Data;
using VezeetaApi.Infrastructure.Repositories;

namespace VezeetaApi.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly VezeetaDbContext DbContext;
        private Dictionary<Type, object> Repositories;

        public UnitOfWork(VezeetaDbContext dbContext)
        {
            DbContext = dbContext;
            Repositories = new Dictionary<Type, object>();
        }

        public IBaseRepository<T> GetRepository<T>() where T : class
        {
            // Check if the Dictionary Key contains the Model class
            if (Repositories.Keys.Contains(typeof(T)))
            {
                // Return the repository for Model class
                return Repositories[typeof(T)] as IBaseRepository<T>;
            }

            // If the dictionary doesn't contain the Model class, create a new repository for the Model class
            IBaseRepository<T> repo = new BaseRepository<T>(DbContext);
            Repositories.Add(typeof(T), repo);
            return repo;
        }

        public async Task<int> SaveChangesAsync()
        {
            var entries = DbContext.ChangeTracker
                .Entries()
                .Where(e => e.Entity.GetType().BaseType != null
                    && e.Entity.GetType().BaseType.IsGenericType
                    && e.Entity.GetType().BaseType.GetGenericTypeDefinition() == typeof(BaseEntity<>)
                    && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                // Get the actual type of the entity
                var entityType = entityEntry.Entity.GetType();
                // Get the PropertyInfo of UpdatedDate
                var updatedDateProperty = entityType.GetProperty("UpdatedDate");
                // Set the value of UpdatedDate
                updatedDateProperty.SetValue(entityEntry.Entity, DateTime.Now);

                if (entityEntry.State == EntityState.Added)
                {
                    // Get the PropertyInfo of CreatedDate
                    var createdDateProperty = entityType.GetProperty("CreatedDate");
                    // Set the value of CreatedDate
                    createdDateProperty.SetValue(entityEntry.Entity, DateTime.Now);
                }
            }
            return await DbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
