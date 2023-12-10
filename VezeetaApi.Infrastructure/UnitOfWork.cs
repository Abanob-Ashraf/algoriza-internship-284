using Microsoft.EntityFrameworkCore;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Services;
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

        public IBaseService<T> GetRepository<T>() where T : class
        {
            if (Repositories.Keys.Contains(typeof(T)))
            {
                return Repositories[typeof(T)] as IBaseService<T>;
            }

            IBaseService<T> repo = new BaseRepository<T>(DbContext);
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
