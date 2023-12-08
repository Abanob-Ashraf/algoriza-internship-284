using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Models;
using VezeetaApi.Domain.Repositories;
using VezeetaApi.Infrastructure.Data;
using VezeetaApi.Infrastructure.Repositories;

namespace VezeetaApi.Infrastructure.RepoServices
{
    public class AppointmentRepoService : BaseRepository<Appointment>, IAppointmentRepo
    {
        readonly VezeetaDbContext DbContext;
        public AppointmentRepoService(VezeetaDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public new async Task<object> FindAsync(Expression<Func<Appointment, bool>> criteria)
        {
            // Your custom implementation here
            var result = await DbContext.Appointments
                .Where(criteria)
                .Select(a => new
                {
                    a.ResevationDate,
                    a.Status,
                    a.PatientIdNavigation.PatientFullName,
                    a.DoctorIdNavigation.DoctorFullName,
                    a.DiscountIdNavigation.DiscountCode 
                }).SingleOrDefaultAsync();

            return result;
        }

    }
}
