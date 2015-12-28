using AppMvc6.Models;
using MR.AspNet.Paging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AppMvc6.ViewModels
{
    /// <summary>
    /// practice/indexビューモデル
    /// </summary>
	public class PracticeIndexViewModel
	{
        public string Tab { get; set; }
        public IPagedList<PracticeViewModel> PracticeViewModelPages { get; set; }
	}
    public class PracticeViewModel
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string Title { get; set; }
        public DateTime BeginDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsAllDay { get; set; }
        public string Place { get; set; }
        public string Note { get; set; }
        public bool IsPublished { get; set; }

        public Status State { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        //public virtual ICollection<Song> Songs { get; set; }
        //public virtual ICollection<Entry> Entries { get; set; }
        //public virtual ICollection<Feedback> Feedbacks { get; set; }
        //public virtual ICollection<Comment> Comments { get; set; }

        // 独自プロパティ
        public IEnumerable<EntryUser> EntryUsers { get; set; }
        public IEnumerable<int> FeedbackUsers { get; set; }
        public IEnumerable<string> SongNames { get; set; }
        public int CommentCount { get; set; }

        // readonly
        public int SongCount => SongNames.Count();
        public int AbsentCount => EntryUsers.Count(e => e.Join == false);
        public int FeedbackCount => FeedbackUsers.Distinct().Count();

        public bool? IsEntry(int userid) => EntryUsers.FirstOrDefault(e => e.UserId == userid)?.Join;
        public bool SentFeedback(int userId) => FeedbackUsers.Contains(userId);
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
    public class EntryUser
    {
        public int UserId { get; set; }
        public bool? Join { get; set; }
    }

    public class PracticeDetailsViewModel
    {
        public string Tab { get; set; }
        public PracticeViewModel PracticeViewModel { get; set; }
        public IPagedList<CommentViewModel> CommentViewModelPages { get; set; }
        public IPagedList<EntryViewModel> EntryViewModelPages { get; set; }
        public ICollection<SongViewModel> SongViewModels { get; set; }
        public ICollection<FeedbackViewModel> FeedbackViewModels { get; set; }
    }


    /// <summary>
    /// practice/create, practice/editフォームモデル
    /// </summary>
    public class PracticeEditFormModel
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }

        [Required]
        public string Title { get; set; }
        public bool IsAllDay { get; set; }
        public string Place { get; set; }
        public string Note { get; set; }
        public bool IsPublished { get; set; }

        // 独自プロパティ
        //public int[] SelectedSongIds { get; set; } = new int[0];
        public IEnumerable<int> SelectedSongIds { get; set; } = new int[0];
        public DateTime BeginDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today;
        public int BeginTime { get; set; }
        public int EndTime { get; set; }
    }

    /// <summary>
    /// practice/feedbackビューモデル
    /// </summary>
	public class PracticeFeedbackViewModel
	{
        public Practice Practice { get; set; }
        public IList<FeedbackFormModel> FeedbackFormModels { get; set; }
	}
    /// <summary>
    /// PracticeFeedbackViewModel内の各feedback編集用フォームモデル
    /// </summary>
	public class FeedbackFormModel
	{
		public int? SongId { get; set; }
		public string SongName { get; set; }
		public string SongParts { get; set; }

		public string Part { get; set; }
		public int Rank { get; set; }
		public string Comment { get; set; }
	}

    /// <summary>
    /// practice/songビューモデル
    /// </summary>
    public class PracticeSongViewModel
    {
        public int id { get; set; }

        public string Name { get; set; }
        public DateTime BeginDate { get; set; }

        public string SongName { get; set; }
        public string SongParts { get; set; }

        public IEnumerable<FeedbackViewModel> FeedbackViewModels { get; set; }
    }
    /// <summary>
    /// PracticeSongViewModel内の各Feedback表示用ビューモデル
    /// </summary>
    public class FeedbackViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserIconPath { get; set; }

        public int? SongId { get; set; }

        public string Part { get; set; }
        public int Rank { get; set; }
        public string Comment { get; set; }

        public string RankIconPath
        {
            get
            {
                switch (Rank)
                {
                    case 1:
                        return "img/system/grade01-on.png";
                    case 2:
                        return "img/system/grade02-on.png";
                    case 3:
                        return "img/system/grade03-on.png";
                    default:
                        return "img/system/rest4-on.png";
                }
            }
        }
    }

    public class EntryViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserIconPath { get; set; }

        public bool? Join { get; set; }
        public string Note { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    public class PracticeTab
    {
        public int Id { get; set; }
        public string Tab { get; set; }
    }

    /// <summary>
    /// practice/settingフォームモデル
    /// </summary>
    public class PracticeSettingFormModel
    {
        public int Id { get; set; }
        public int GroupId { get; set; }

        public IEnumerable<int> SelectedSongIds { get; set; } = new int[0];
    }
}