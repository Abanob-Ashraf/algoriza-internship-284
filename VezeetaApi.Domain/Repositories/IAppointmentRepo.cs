using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Services;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Domain.Repositories
{
    public interface IAppointmentRepo : IBaseService<Appointment>
    {
        new Task<object> FindAsync(Expression<Func<Appointment, bool>> criteria);
    }
}
