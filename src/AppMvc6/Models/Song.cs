using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Models
{
    public class Song
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        public string Name { get; set; }
        public string Lyric { get; set; }
        public string Compose { get; set; }
        public string Arrange { get; set; }

        public SongProgress Progress { get; set; }
        public string Parts { get; set; }
        public int Difficulty { get; set; }
        public string Note { get; set; }

        //public virtual ICollection<Practice> Practices { get; set; }
        public virtual ICollection<SongPractice> SongPractices { get; set; }
        public virtual IEnumerable<Practice> Practices
        {
            get { return SongPractices.Select(s => s.Practice); }
        }
        //public virtual ICollection<SongSchedule> SongSchedules { get; set; }
        //public virtual ICollection<Topic> Topics { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; }
    }
    public class SongPractice
    {
        public int SongId { get; set; }
        public virtual Song Song { get; set; }

        public int PracticeId { get; set; }
        public virtual Practice Practice { get; set; }
    }
    public static class PartExtention
    {
        /// <summary>
        /// string から Web.Mvc.SelectListItem のリストを作成するための拡張メソッド
        /// </summary>
        /// <param name="value">カンマ区切り文字列</param>
        /// <param name="select">初期選択されるテキスト</param>
        /// <returns></returns>
		public static IEnumerable<SelectListItem> ToSelectList(this string value, string select)
        {
            var list = new List<SelectListItem>();
            if (value != null)
            {
                foreach (string part in value.Split(','))
                {
                    list.Add(new SelectListItem { Text = part, Value = part, Selected = (part == select) });
                }
            }

            return list;
        }
    }
}
