using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Infrastructure.Repositories
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual List<RefreshToken>? RefreshTokens { get; set; }

    }
}
