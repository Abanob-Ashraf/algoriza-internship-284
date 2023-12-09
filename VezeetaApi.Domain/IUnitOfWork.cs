using VezeetaApi.Domain.Services;
using VezeetaApi.Domain.Repositories;

namespace VezeetaApi.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseService<T> GetRepository<T>() where T : class;

        IAppointmentRepo AppointmentRepo { get; }
        Task<int> SaveChangesAsync();
    }
}
