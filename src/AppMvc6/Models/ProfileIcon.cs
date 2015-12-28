using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Models
{
    public class ProfileIcon
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }

        public string Path { get; set; }
        //public bool Selected { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; }


        public ProfileIcon()
        {
        }
        public ProfileIcon(int userId, string extension)
        {
            UserId = userId;
            Path = "img/user/" + userId + "/" + Guid.NewGuid().ToString("N") + extension;
        }
        public ProfileIcon(Group group, string extension)
        {
            GroupId = group.Id;
            Path = "img/group/" + group.Account + "/" + Guid.NewGuid().ToString("N") + extension;
        }
    }
}
