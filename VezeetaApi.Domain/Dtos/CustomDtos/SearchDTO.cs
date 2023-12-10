using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaApi.Domain.Dtos.CustomDtos
{
    public class SearchDTO
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? SpecializationName { get; set; }
    }
}
