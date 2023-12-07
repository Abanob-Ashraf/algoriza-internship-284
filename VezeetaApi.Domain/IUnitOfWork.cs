using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
