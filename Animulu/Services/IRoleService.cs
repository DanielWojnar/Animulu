using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Animulu.Services
{
    public interface IRoleService
    {
        public Task<IEnumerable<IdentityUserRole<string>>> GetAllUsersAsync();
    }
}
