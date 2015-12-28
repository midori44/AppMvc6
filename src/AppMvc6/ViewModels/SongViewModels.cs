using AppMvc6.Models;
using MR.AspNet.Paging;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppMvc6.ViewModels
{
    public class SongListViewModel
    {
        public IPagedList<Song> Songs { get; set; }
        public SongProgress Progress { get; set; }
    }
    public class SongViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Lyric { get; set; }
        public string Compose { get; set; }
        public string Arrange { get; set; }
        public SongProgress Progress { get; set; }
        public string Parts { get; set; }
        public string Note { get; set; }

        // 独自プロパティ
        public int PracticeCount { get; set; }
        public int CommentCount { get; set; }
    }

    public class SongDetailViewModel
    {
        public string Tab { get; set; }
        public SongViewModel SongViewModel { get; set; }
        public IPagedList<ReportViewModel> ReportPages { get; set; }
        public IPagedList<CommentViewModel> CommentViewModelPages { get; set; }
    }
    public class ReportViewModel
    {
        public string UserName { get; set; }
        public string IconPath { get; set; }
        public string Part { get; set; }
        public int Count { get; set; }
        public int? LastPracticeId { get; set; }
        public DateTime? LastAttend { get; set; }
        public string LastRankIconPath { get; set; }
    }


    public class SongEditFormModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Lyric { get; set; }
        public string Compose { get; set; }
        public string Arrange { get; set; }
        public SongProgress Progress { get; set; } = SongProgress.Going;
        public string Note { get; set; }

        public string[] SelectedParts { get; set; } = new string[0];
    }

    
}