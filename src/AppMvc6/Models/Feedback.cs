using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int? SongId { get; set; }
        public virtual Song Song { get; set; }
        public int PracticeId { get; set; }
        public virtual Practice Practice { get; set; }

        public string Part { get; set; }
        public int Rank { get; set; }
        public string Comment { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; }
    }
}
