using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Interfaces;
using VezeetaApi.Infrastructure.Data;

namespace VezeetaApi.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly VezeetaDbContext DbContext;

        public BaseRepository(VezeetaDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria)
        {
            return await DbContext.Set<T>().SingleOrDefaultAsync(criteria);
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = DbContext.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.Where(criteria).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await DbContext.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public void Update(T entity)
        {
            DbContext.Set<T>().Update(entity);
        }

        public T Delete(int id)
        {
            var entity = DbContext.Set<T>().Find(id);
            if (entity is not null)
                DbContext.Set<T>().Remove(entity);
            return entity;
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            DbContext.Set<T>().RemoveRange(entities);
        }
    }
}
