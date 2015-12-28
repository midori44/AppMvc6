using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Models
{
    public class Member
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Role Role { get; set; }
        public string Comment { get; set; }

        public Status State { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; }
    }
}
