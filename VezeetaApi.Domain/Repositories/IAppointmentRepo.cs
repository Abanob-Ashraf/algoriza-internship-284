using System.Linq.Expressions;
using VezeetaApi.Domain.Services;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Domain.Repositories
{
    public interface IAppointmentRepo : IBaseService<Appointment>
    {
        new Task<object> FindAsync(Expression<Func<Appointment, bool>> criteria);
    }
}
