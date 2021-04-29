using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Data;
using Animulu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Animulu.Services
{
    public class RoleService
    {
        private readonly IdentityContext _context;
        public RoleService(IdentityContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<IdentityUserRole<string>>> GetAllUsersAsync(){
            var result = await (from ur in _context.UserRoles
                                select ur).AsNoTracking().ToListAsync();
            return result;
        }

    }
}
