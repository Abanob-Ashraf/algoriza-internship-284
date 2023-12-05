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
        readonly VezeetaDbContext DbContext;
        private readonly DbSet<T> DbSet;

        public BaseRepository(VezeetaDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria)
        {
            return await DbSet.SingleOrDefaultAsync(criteria);
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = DbSet;

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.Where(criteria).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await DbSet.AddRangeAsync(entities);
            return entities;
        }

        public void Update(T entity)
        {
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public T Delete(int id)
        {
            var entity = DbSet.Find(id);
            if (entity is not null)
                DbSet.Remove(entity);
            return entity;
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }
    }
}
