using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Models
{
    public class Practice
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public string Title { get; set; }
        public DateTime BeginDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsAllDay { get; set; }
        public string Place { get; set; }
        public string Note { get; set; }
        public bool IsPublished { get; set; }

        //public virtual ICollection<Song> Songs { get; set; }
        public virtual ICollection<SongPractice> SongPractices { get; set; }
        public virtual IEnumerable<Song> Songs
        {
            get { return SongPractices.Select(s => s.Song); }
            set { SongPractices = (ICollection<SongPractice>)value.Select(s => s.SongPractices); }
        }
        public virtual ICollection<Entry> Entries { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public Status State { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; }


        public string DisplayDateTime()
        {
            if (IsAllDay)
            {
                string begin = BeginDateTime.ToString("yyyy年M月d日 ddd曜日");
                if (BeginDateTime.Date == EndDateTime.Date)
                {
                    return begin;
                }
                else if (BeginDateTime.Year == EndDateTime.Year)
                {
                    string end = EndDateTime.ToString("M月d日 ddd曜日");
                    return begin + " ～ " + end;
                }
                else
                {
                    string end = EndDateTime.ToString("yyyy年M月d日 ddd曜日");
                    return begin + " ～ " + end;
                }
            }
            else
            {
                string begin = BeginDateTime.ToString("yyyy年M月d日 ddd曜日 HH:mm");
                if (BeginDateTime.Date == EndDateTime.Date)
                {
                    string end = EndDateTime.ToString("HH:mm");
                    return begin + " ～ " + end;
                }
                else if (BeginDateTime.Year == EndDateTime.Year)
                {
                    string end = EndDateTime.ToString("M月d日 ddd曜日 HH:mm");
                    return begin + " ～ " + end;
                }
                else
                {
                    string end = EndDateTime.ToString("yyyy年M月d日 ddd曜日 HH:mm");
                    return begin + " ～ " + end;
                }
            }
        }
    }
}
