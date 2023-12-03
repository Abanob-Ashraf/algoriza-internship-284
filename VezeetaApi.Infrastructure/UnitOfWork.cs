using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Interfaces;
using VezeetaApi.Infrastructure.Data;
using VezeetaApi.Infrastructure.Repositories;

namespace VezeetaApi.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VezeetaDbContext DbContext;
        private Dictionary<Type, object> Repositories;

        public UnitOfWork(VezeetaDbContext dbContext)
        {
            DbContext = dbContext;
            Repositories = new Dictionary<Type, object>();
        }

        public IBaseRepository<T> GetRepository<T>() where T : class
        {
            // Check if the Dictionary Key contains the Model class
            if (Repositories.Keys.Contains(typeof(T)))
            {
                // Return the repository for Model class
                return Repositories[typeof(T)] as IBaseRepository<T>;
            }

            // If the dictionary doesn't contain the Model class, create a new repository for the Model class
            IBaseRepository<T> repo = new BaseRepository<T>(DbContext);
            Repositories.Add(typeof(T), repo);
            return repo;
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public void Dispose()
        {
            DbContext.Dispose();
            Repositories = null;
        }
    }
}
