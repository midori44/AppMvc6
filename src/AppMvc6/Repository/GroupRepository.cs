using AppMvc6.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Repository
{
    public class GroupRepository
    {
        private readonly ApplicationDbContext DbContext;
        public GroupRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            return await DbContext.Users.ToListAsync();
        }
    }
}
