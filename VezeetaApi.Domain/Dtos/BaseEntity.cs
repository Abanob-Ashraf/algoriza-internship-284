using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaApi.Domain.Dtos
{
    public class BaseEntity<T>
    {
        public T Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
