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
            if (Repositories.Keys.Contains(typeof(T)))
            {
                return Repositories[typeof(T)] as IBaseRepository<T>;
            }

            IBaseRepository<T> repo = new BaseRepository<T>(DbContext);
            Repositories.Add(typeof(T), repo);
            return repo;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
