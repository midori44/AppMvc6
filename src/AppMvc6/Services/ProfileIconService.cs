using AppMvc6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Services
{
    public interface IProfileIconService
    {
        ProfileIcon GetUserIcon(int id, int userId);
        ProfileIcon GetGroupIcon(int id, int groupId);
        IEnumerable<ProfileIcon> GetGroupIcons(int groupId);
        void CreateIcon(ProfileIcon icon);
        void DeleteIcon(ProfileIcon icon);
    }
    public class ProfileIconService : IProfileIconService
    {
        public void CreateIcon(ProfileIcon icon)
        {
            throw new NotImplementedException();
        }

        public void DeleteIcon(ProfileIcon icon)
        {
            throw new NotImplementedException();
        }

        public ProfileIcon GetGroupIcon(int id, int groupId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProfileIcon> GetGroupIcons(int groupId)
        {
            throw new NotImplementedException();
        }

        public ProfileIcon GetUserIcon(int id, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
