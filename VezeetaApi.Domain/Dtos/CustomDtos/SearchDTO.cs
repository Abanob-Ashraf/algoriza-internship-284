using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }
        public string? SpecializationName { get; set; }

        public DateTime? ResevationDate { get; set; }
    }
}
