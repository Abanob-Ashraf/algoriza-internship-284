using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VezeetaApi.Domain.Services;
using VezeetaApi.Infrastructure.Data;

namespace VezeetaApi.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseService<T> where T : class
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
            return await DbSet.AsNoTracking().SingleOrDefaultAsync(criteria);
        }

        public async Task<IEnumerable<T>> FindAllAsyncPaginated(Expression<Func<T, bool>> criteria, int page = 1, int PageSize = 5)
        {
            IQueryable<T> query = DbSet;

            if (criteria is not null)
            {
                query = query.Where(criteria);
            }

            return await query.Skip((page-1) * PageSize).Take(PageSize).ToListAsync();
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
            DbSet.Update(entity);
            //DbContext.Entry(entity).State = EntityState.Modified;
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

        public void DeActiveAndActive(IActivatable entity)
        {
            entity.IsActive = !entity.IsActive;
            DbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
