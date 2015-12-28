using AppMvc6.Infrastructure;
using AppMvc6.Models;
using AppMvc6.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using MR.AspNet.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Services
{
    public interface IPracticeService
    {
        Practice GetPractice(int practiceId, int groupId);
        PracticeViewModel GetPracticeViewModel(int practiceId, int groupId);
        IPagedList<PracticeViewModel> GetPracticeViewModelPages(int groupId, string tab, Page page);
        IEnumerable<PracticeViewModel> GetTodayPracticeViewModels(IEnumerable<Group> groups);
        PracticeEditFormModel GetPracticeEditFormModel(int practiceId, int groupId);
        void CreatePractice(PracticeEditFormModel formModel);
        void UpdatePractice(PracticeEditFormModel formModel);
        void DeletePractice(Practice practice);

        IList<FeedbackFormModel> GetFeedbackFormModels(Practice practice, int userId);
        ICollection<FeedbackViewModel> GetFeedbackViewModels(int practiceId);
        void UpdateFeedbacks(IEnumerable<FeedbackFormModel> formModels, int practiceId, int userId);
        PracticeSongViewModel GetPracticeSongViewModel(Practice practice, Song song);

        IEnumerable<SelectListItem> GetTimesSelectList();
        IEnumerable<Song> GetActiveSongs(int groupId, int practiceId);

        IPagedList<EntryViewModel> GetEntryViewModelPages(int practiceId, Page page);
        void UpdateEntry(int practiceId, int userId, bool? join);

        IPagedList<CommentViewModel> GetCommentViewModelPages(int practiceId, Page page);
    }
    public class PracticeService : IPracticeService
    {
        private readonly IRepository<Practice> practiceRepository;
        private readonly IRepository<Song> songRepository;
        private readonly IRepository<Feedback> feedbackRepository;
        private readonly IRepository<Entry> entryRepository;
        private readonly IRepository<Comment> commentRepository;
        private readonly IUnitOfWork unitOfWork;

        public PracticeService(IUnitOfWork unitOfWork)
        {
            this.practiceRepository = unitOfWork.Repository<Practice>();
            this.songRepository = unitOfWork.Repository<Song>();
            this.feedbackRepository = unitOfWork.Repository<Feedback>();
            this.entryRepository = unitOfWork.Repository<Entry>();
            this.commentRepository = unitOfWork.Repository<Comment>();
            this.unitOfWork = unitOfWork;
        }


        public Practice GetPractice(int practiceId, int groupId)
        {
            var practice = practiceRepository
                .Get(p => p.Id == practiceId && p.GroupId == groupId);

            return practice;
        }

        public PracticeViewModel GetPracticeViewModel(int practiceId, int groupId)
        {
            Mapper.CreateMap<Practice, PracticeViewModel>()
                .ForMember(m => m.SongNames, o => o.MapFrom(p => p.Songs.Select(s => s.Name)))
                .ForMember(m => m.CommentCount, o => o.MapFrom(p => p.Comments.Count()))
                .ForMember(m => m.EntryUsers, o => o.MapFrom(p => p.Entries.Select(e => new EntryUser { UserId = e.UserId, Join = e.Join })))
                .ForMember(m => m.FeedbackUsers, o => o.MapFrom(p => p.Feedbacks.Where(f => f.Rank > 0).Select(f => f.UserId)));

            var practices = practiceRepository
                 .Where(p => p.Id == practiceId)
                 .Where(p => p.GroupId == groupId)
                 .Where(p => p.State == Status.Public)
                 .OrderByDescending(p => p.Id)
                 .ProjectTo<PracticeViewModel>();

            return practices.FirstOrDefault();
        }

        public IPagedList<PracticeViewModel> GetPracticeViewModelPages(int groupId, string tab, Page page)
        {
            Mapper.CreateMap<Practice, PracticeViewModel>()
                .ForMember(m => m.SongNames, o => o.MapFrom(p => p.Songs.Select(s => s.Name)))
                .ForMember(m => m.CommentCount, o => o.MapFrom(p => p.Comments.Count()))
                .ForMember(m => m.EntryUsers, o => o.MapFrom(p => p.Entries.Select(e => new EntryUser { UserId = e.UserId, Join = e.Join })))
                .ForMember(m => m.FeedbackUsers, o => o.MapFrom(p => p.Feedbacks.Where(f => f.Rank > 0).Select(f => f.UserId)));

            var practices = practiceRepository
                .Where(p => p.GroupId == groupId)
                .Where(p => p.State == Status.Public);

            practices = (tab != "closed")
                ? practices.Where(p => p.BeginDateTime >= DateTime.Today)
                    .OrderBy(p => p.BeginDateTime)
                : practices.Where(p => p.BeginDateTime < DateTime.Now)
                    .OrderByDescending(p => p.BeginDateTime);

            var viewModel = practices
                .ProjectTo<PracticeViewModel>();

            return viewModel.ToPagedList(page);
        }

        public IEnumerable<PracticeViewModel> GetTodayPracticeViewModels(IEnumerable<Group> groups)
        {
            Mapper.CreateMap<Practice, PracticeViewModel>()
                .ForMember(m => m.SongNames, o => o.MapFrom(p => p.Songs.Select(s => s.Name)))
                .ForMember(m => m.CommentCount, o => o.MapFrom(p => p.Comments.Count()))
                .ForMember(m => m.EntryUsers, o => o.MapFrom(p => p.Entries.Select(e => new EntryUser { UserId = e.UserId, Join = e.Join })))
                .ForMember(m => m.FeedbackUsers, o => o.MapFrom(p => p.Feedbacks.Where(f => f.Rank > 0).Select(f => f.UserId)));

            int[] groupIds = groups.Select(g => g.Id).ToArray();
            DateTime today = DateTime.Today;
            DateTime tomorrow = DateTime.Today.AddDays(1);

            var practices = practiceRepository
                 .Where(p => groupIds.Contains(p.GroupId))
                 .Where(p => p.State == Status.Public)
                 .Where(p => p.BeginDateTime < tomorrow && p.EndDateTime >= today)
                 .OrderBy(p => p.BeginDateTime)
                 .ProjectTo<PracticeViewModel>();

            return practices.ToList();
        }

        public PracticeEditFormModel GetPracticeEditFormModel(int practiceId, int groupId)
        {
            Mapper.CreateMap<Practice, PracticeEditFormModel>()
               .ForMember(m => m.BeginDate, o => o.MapFrom(p => p.BeginDateTime))
               .ForMember(m => m.EndDate, o => o.MapFrom(p => p.EndDateTime))
               .ForMember(m => m.BeginTime, o => o.MapFrom(p => (p.BeginDateTime.Hour * 2) + (p.BeginDateTime.Minute / 30)))
               .ForMember(m => m.EndTime, o => o.MapFrom(p => (p.EndDateTime.Hour * 2) + (p.EndDateTime.Minute / 30)))
               .ForMember(m => m.SelectedSongIds, o => o.MapFrom(p => p.Songs.Select(s => s.Id)));

            var practices = practiceRepository
                .Where(p => p.Id == practiceId && p.GroupId == groupId)
                .OrderBy(p => p.Id)
                .ProjectTo<PracticeEditFormModel>();

            return practices.FirstOrDefault();
        }

        public void CreatePractice(PracticeEditFormModel formModel)
        {
            Mapper.CreateMap<PracticeEditFormModel, Practice>()
                .ForMember(m => m.BeginDateTime, o => o.MapFrom(p => p.BeginDate.Date.AddHours(p.BeginTime / 2).AddMinutes(30 * (p.BeginTime % 2))))
                .ForMember(m => m.EndDateTime, o => o.MapFrom(p => p.EndDate.Date.AddHours(p.EndTime / 2).AddMinutes(30 * (p.EndTime % 2))));
            Practice practice = Mapper.Map<Practice>(formModel);
            practice.Songs = songRepository
                .Where(s => formModel.SelectedSongIds.Contains(s.Id))
                .ToList();

            practiceRepository.Insert(practice);
            unitOfWork.Commit();
        }

        public void UpdatePractice(PracticeEditFormModel formModel)
        {
            var beforePractice = practiceRepository
                .Where(p => p.State == Status.Public)
                .Include(p => p.Songs)
                .FirstOrDefault(p => p.Id == formModel.Id);
            if (beforePractice == null)
            {
                return;
            }

            var beforeSongs = beforePractice.Songs;
            var afterSongs = songRepository
                .Where(s => formModel.SelectedSongIds.Contains(s.Id))
                .ToList();

            Mapper.CreateMap<PracticeEditFormModel, Practice>()
                .ForMember(m => m.BeginDateTime, o => o.MapFrom(p => p.BeginDate.Date.AddHours(p.BeginTime / 2).AddMinutes(30 * (p.BeginTime % 2))))
                .ForMember(m => m.EndDateTime, o => o.MapFrom(p => p.EndDate.Date.AddHours(p.EndTime / 2).AddMinutes(30 * (p.EndTime % 2))));
            var practice = Mapper.Map(formModel, beforePractice);
            practice.Songs = afterSongs
                .Select(after => beforeSongs.FirstOrDefault(before => before.Id == after.Id) ?? after)
                .ToList();

            practiceRepository.Update(practice);
            unitOfWork.Commit();
        }

        public void DeletePractice(Practice practice)
        {
            feedbackRepository.Delete(f => f.PracticeId == practice.Id);
            commentRepository.Delete(c => c.PracticeId == practice.Id);
            entryRepository.Delete(e => e.PracticeId == practice.Id);
            practiceRepository.Delete(practice);
            unitOfWork.Commit();
        }


        public IList<FeedbackFormModel> GetFeedbackFormModels(Practice practice, int userId)
        {
            Mapper.CreateMap<Feedback, FeedbackFormModel>();
            var feedbackList = feedbackRepository.Where(f => f.UserId == userId).ToList();
            var feedbacks = practice.Songs
                .Select(s => feedbackList.FirstOrDefault(f => f.PracticeId == practice.Id && f.SongId == s.Id)
                    ?? new Feedback { SongId = s.Id, Song = s, Part = latestPart(feedbackList, s.Id) })
                .ToList();

            var totalFeedback = feedbackList.FirstOrDefault(f => f.PracticeId == practice.Id && f.SongId == null)
                    ?? new Feedback { SongId = null };
            feedbacks.Add(totalFeedback);

            var viewModel = Mapper.Map<IList<FeedbackFormModel>>(feedbacks);

            return viewModel;
        }
        private string latestPart(IEnumerable<Feedback> feedbackList, int songId)
        {
            // 最新練習の登録パート取得
            Feedback feedback = feedbackList
                .Where(f => f.SongId == songId)
                .OrderByDescending(f => f.Practice.BeginDateTime)
                .FirstOrDefault();

            return feedback?.Part;
        }

        public ICollection<FeedbackViewModel> GetFeedbackViewModels(int practiceId)
        {
            Mapper.CreateMap<Feedback, FeedbackViewModel>();
            var feedbacks = feedbackRepository
                .Where(f => f.PracticeId == practiceId)
                .OrderBy(f => f.Created)
                .ProjectTo<FeedbackViewModel>();

            return feedbacks.ToList();
        }

        public void UpdateFeedbacks(IEnumerable<FeedbackFormModel> formModels, int practiceId, int userId)
        {
            Mapper.CreateMap<FeedbackFormModel, Feedback>();
            var feedbacks = feedbackRepository
                .Where(f => f.PracticeId == practiceId)
                .Where(f => f.UserId == userId)
                .ToList();

            foreach (var formModel in formModels)
            {
                // 曲評価の登録
                var feedback = feedbacks.FirstOrDefault(f => f.SongId == formModel.SongId);
                if (formModel.Rank > 0 || !string.IsNullOrWhiteSpace(formModel.Comment))
                {
                    // 評価またはコメントありのとき
                    if (feedback == null)
                    {
                        // 未登録のとき新規登録
                        feedback = Mapper.Map<Feedback>(formModel);
                        feedback.PracticeId = practiceId;
                        feedback.UserId = userId;
                        feedbackRepository.Insert(feedback);
                    }
                    else if (feedback.Rank != formModel.Rank || feedback.Comment != formModel.Comment || feedback.Part != formModel.Part)
                    {
                        // 登録済みかつ変更ありのとき更新
                        feedback = Mapper.Map(formModel, feedback);
                        feedbackRepository.Update(feedback);
                    }

                }
                else if (feedback != null)
                {
                    // 評価なしコメントなしで登録済みのとき削除
                    feedbackRepository.Delete(feedback);
                }

            }

            unitOfWork.Commit();

            //// Entry更新（評価の付いたfeedbackが1つでもあれば参加）
            //bool join = formModels.Any(f => f.Rank > 0);
            //UpdateEntry(practiceId, userId, join);
        }

        public PracticeSongViewModel GetPracticeSongViewModel(Practice practice, Song song)
        {
            Mapper.CreateMap<Practice, PracticeSongViewModel>();
            Mapper.CreateMap<Feedback, FeedbackViewModel>();
            var feedbacks = feedbackRepository
                .Where(f => f.PracticeId == practice.Id)
                .Where(f => f.SongId == song.Id)
                .OrderBy(f => f.Created)
                .ToList();

            var viewModel = Mapper.Map<PracticeSongViewModel>(practice);
            viewModel.SongName = song.Name;
            viewModel.SongParts = song.Parts;
            viewModel.FeedbackViewModels = Mapper.Map<IEnumerable<FeedbackViewModel>>(feedbacks);

            return viewModel;
        }


        public IEnumerable<SelectListItem> GetTimesSelectList()
        {
            var list = new SelectListItem[]
            {
                new SelectListItem { Text = "00:00", Value = "0" },
                new SelectListItem { Text = "00:30", Value = "1" },
                new SelectListItem { Text = "01:00", Value = "2" },
                new SelectListItem { Text = "01:30", Value = "3" },
                new SelectListItem { Text = "02:00", Value = "4" },
                new SelectListItem { Text = "02:30", Value = "5" },
                new SelectListItem { Text = "03:00", Value = "6" },
                new SelectListItem { Text = "03:30", Value = "7" },
                new SelectListItem { Text = "04:00", Value = "8" },
                new SelectListItem { Text = "04:30", Value = "9" },
                new SelectListItem { Text = "05:00", Value = "10" },
                new SelectListItem { Text = "05:30", Value = "11" },
                new SelectListItem { Text = "06:00", Value = "12" },
                new SelectListItem { Text = "06:30", Value = "13" },
                new SelectListItem { Text = "07:00", Value = "14" },
                new SelectListItem { Text = "07:30", Value = "15" },
                new SelectListItem { Text = "08:00", Value = "16" },
                new SelectListItem { Text = "08:30", Value = "17" },
                new SelectListItem { Text = "09:00", Value = "18" },
                new SelectListItem { Text = "09:30", Value = "19" },
                new SelectListItem { Text = "10:00", Value = "20" },
                new SelectListItem { Text = "10:30", Value = "21" },
                new SelectListItem { Text = "11:00", Value = "22" },
                new SelectListItem { Text = "11:30", Value = "23" },
                new SelectListItem { Text = "12:00", Value = "24" },
                new SelectListItem { Text = "12:30", Value = "25" },
                new SelectListItem { Text = "13:00", Value = "26" },
                new SelectListItem { Text = "13:30", Value = "27" },
                new SelectListItem { Text = "14:00", Value = "28" },
                new SelectListItem { Text = "14:30", Value = "29" },
                new SelectListItem { Text = "15:00", Value = "30" },
                new SelectListItem { Text = "15:30", Value = "31" },
                new SelectListItem { Text = "16:00", Value = "32" },
                new SelectListItem { Text = "16:30", Value = "33" },
                new SelectListItem { Text = "17:00", Value = "34" },
                new SelectListItem { Text = "17:30", Value = "35" },
                new SelectListItem { Text = "18:00", Value = "36" },
                new SelectListItem { Text = "18:30", Value = "37" },
                new SelectListItem { Text = "19:00", Value = "38" },
                new SelectListItem { Text = "19:30", Value = "39" },
                new SelectListItem { Text = "20:00", Value = "40" },
                new SelectListItem { Text = "20:30", Value = "41" },
                new SelectListItem { Text = "21:00", Value = "42" },
                new SelectListItem { Text = "21:30", Value = "43" },
                new SelectListItem { Text = "22:00", Value = "44" },
                new SelectListItem { Text = "22:30", Value = "45" },
                new SelectListItem { Text = "23:00", Value = "46" },
                new SelectListItem { Text = "23:30", Value = "47" }
            };

            return list;
        }

        public IEnumerable<Song> GetActiveSongs(int groupId, int practiceId)
        {
            var songs = songRepository
                .Where(s => s.GroupId == groupId)
                .Where(s => s.Progress == SongProgress.Going || s.Practices.Any(p => p.Id == practiceId));

            return songs.ToList();
        }


        public IPagedList<EntryViewModel> GetEntryViewModelPages(int practiceId, Page page)
        {
            Mapper.CreateMap<Entry, EntryViewModel>();

            var entries = entryRepository
                .Where(e => e.PracticeId == practiceId)
                .Where(e => e.Join == false)
                .OrderBy(e => e.Created)
                .ProjectTo<EntryViewModel>();

            return entries.ToPagedList(page);
        }

        public void UpdateEntry(int practiceId, int userId, bool? join)
        {
            var entry = entryRepository
                .Where(e => e.PracticeId == practiceId)
                .Where(e => e.UserId == userId)
                .FirstOrDefault();

            if (entry == null && join != null)
            {
                // 未登録かつ保留でないとき新規作成
                entry = new Entry { PracticeId = practiceId, UserId = userId, Join = join };
                entryRepository.Insert(entry);
                unitOfWork.Commit();
            }
            else if (entry != null && entry.Join != join)
            {
                // 登録済みかつ変更ありのとき更新
                entry.Join = join;
                entryRepository.Update(entry);
                unitOfWork.Commit();
            }
        }


        public IPagedList<CommentViewModel> GetCommentViewModelPages(int practiceId, Page page)
        {
            Mapper.CreateMap<Comment, CommentViewModel>();

            var comments = commentRepository
                .Where(c => c.PracticeId == practiceId)
                .Where(c => c.State == Status.Public)
                .OrderByDescending(c => c.Created)
                .ProjectTo<CommentViewModel>();

            return comments.ToPagedList(page);
        }


    }
}
