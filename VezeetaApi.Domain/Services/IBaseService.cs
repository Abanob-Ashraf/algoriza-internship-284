using System.Linq.Expressions;

namespace VezeetaApi.Domain.Services
{
    public interface IBaseService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task<T> FindAsync(Expression<Func<T, bool>> criteria);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null);

        Task<T> AddAsync(T entity);

        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        T Delete(int id);

        void DeleteRange(IEnumerable<T> entities);

        void DeActiveAndActive(IActivatable entity);
    }
}
