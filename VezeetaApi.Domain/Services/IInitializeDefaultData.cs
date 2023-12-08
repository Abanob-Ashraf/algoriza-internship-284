using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaApi.Domain.Services
{
    public interface IInitializeDefaultData
    {
        public Task InitializeData();
    }
}
