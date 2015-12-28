using AppMvc6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Services
{
    public interface IUserService
    {
        ApplicationUser GetUser(int id);
        void UpdateUser(ApplicationUser user);
        void DeleteUser(int userId);

        bool CanUseEmail(int userId, string email);
    }
    public class UserService : IUserService
    {
        public bool CanUseEmail(int userId, string email)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
