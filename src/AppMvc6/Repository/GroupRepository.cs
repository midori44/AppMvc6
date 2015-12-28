using AppMvc6.Models;
using AppMvc6.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Repository
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetGroupsAsync(int userId);
    }
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public Task<IEnumerable<Group>> GetGroupsAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
