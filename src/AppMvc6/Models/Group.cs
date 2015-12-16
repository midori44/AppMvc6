using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string IconPath { get; set; } = "images/group/default.png";

        public Voice Voice { get; set; }
        public bool IsStudent { get; set; }
        public int? NumberOfMembers { get; set; }
        public string Profile { get; set; }

        public Area Area { get; set; }
        public string AreaDetail { get; set; }

        public Activity Activity { get; set; }

        public bool NowHiring { get; set; }
        public string HiringComment { get; set; }

        public bool DayMon { get; set; }
        public bool DayTue { get; set; }
        public bool DayWed { get; set; }
        public bool DayThu { get; set; }
        public bool DayFri { get; set; }
        public bool DaySat { get; set; }
        public bool DaySun { get; set; }
        public bool DayOther { get; set; }

        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string WebSite { get; set; }

        //public virtual ICollection<Article> Articles { get; set; }
        //public virtual ICollection<Chat> Chats { get; set; }
        //public virtual ICollection<Event> Events { get; set; }
        //public virtual ICollection<Favorite> Favs { get; set; }
        //public virtual ICollection<Member> Members { get; set; }
        //public virtual ICollection<Movie> Movies { get; set; }
        //public virtual ICollection<Practice> Practices { get; set; }
        //public virtual ICollection<ProfileIcon> ProfileIcons { get; set; }
        //public virtual ICollection<Song> Songs { get; set; }
        //public virtual ICollection<Tag> Tags { get; set; }
        //public virtual ICollection<Topic> Topics { get; set; }

        //public Status State { get; set; } = Status.Public;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; }



        //public bool IsMember(int userId)
        //{
        //    return Members.Any(m => m.UserId == userId && m.State == Status.Public);
        //}
        //public bool IsFav(int userId)
        //{
        //    return Favs.Any(f => f.UserId == userId);
        //}
    }
}
