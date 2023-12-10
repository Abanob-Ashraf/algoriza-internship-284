using VezeetaApi.Domain.Services;

namespace VezeetaApi.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseService<T> GetRepository<T>() where T : class;

        Task<int> SaveChangesAsync();
    }
}
