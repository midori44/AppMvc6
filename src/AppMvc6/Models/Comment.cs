using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        //public int? ArticleId { get; set; }
        //public virtual Article Atricle { get; set; }
        public int? PracticeId { get; set; }
        public virtual Practice Practice { get; set; }
        //public int? TopicId { get; set; }
        //public virtual Topic Topic { get; set; }

        public int? SongId { get; set; }
        public virtual Song Song { get; set; }

        public string Content { get; set; }

        public Status State { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; }
    }
}
