using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Models
{
    public class Entry
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        //public int? EventId { get; set; }
        //public virtual Event Event { get; set; }
        public int? PracticeId { get; set; }
        public virtual Practice Practice { get; set; }

        public bool? Join { get; set; }
        public string Note { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; }


        //public Entry()
        //{
        //    Created = Modified = DateTime.Now;
        //}
        //public Entry(EntryType type, int? id, int userId)
        //{
        //    UserId = userId;
        //    EventId = (type == EntryType.Event) ? id : null;
        //    PracticeId = (type == EntryType.Practice) ? id : null;
        //    Created = Modified = DateTime.Now;
        //}
    }
}
