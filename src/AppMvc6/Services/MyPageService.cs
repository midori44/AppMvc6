using AppMvc6.Models;
using AppMvc6.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Services
{
    public interface IMyPageService
    {
        Task<MyPageIndexViewModel> GetMyPageIndexViewModelAsync(int userId);
    }
    public class MyPageService : IMyPageService
    {
        private readonly IUnitOfWork unitOfWork;
        //private readonly IRepository<ApplicationUser> userRepository;
        private readonly IRepository<Group> groupRepository;
        private readonly IRepository<Member> memberRepository;
        private readonly IRepository<Practice> practiceRepository;
        public MyPageService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            //userRepository = unitOfWork.Repository<ApplicationUser>();
            groupRepository = unitOfWork.Repository<Group>();
            memberRepository = unitOfWork.Repository<Member>();
            practiceRepository = unitOfWork.Repository<Practice>();
        }

        public async Task<MyPageIndexViewModel> GetMyPageIndexViewModelAsync(int userId)
        {
            var groups = await memberRepository
                .Where(m => m.UserId == userId && m.State == Status.Public)
                .OrderByDescending(m => m.Created)
                .Select(m => m.Group)
                .Where(g => g.State == Status.Public)
                .ToListAsync();

            Mapper.CreateMap<Practice, PracticeViewModel>()
                .ForMember(m => m.SongNames, o => o.MapFrom(p => p.Songs.Select(s => s.Name)))
                .ForMember(m => m.CommentCount, o => o.MapFrom(p => p.Comments.Count()))
                .ForMember(m => m.EntryUsers, o => o.MapFrom(p => p.Entries.Select(e => new EntryUser { UserId = e.UserId, Join = e.Join })))
                .ForMember(m => m.FeedbackUsers, o => o.MapFrom(p => p.Feedbacks.Where(f => f.Rank > 0).Select(f => f.UserId)));

            int[] groupIds = groups.Select(g => g.Id).ToArray();
            DateTime today = DateTime.Today;
            DateTime tomorrow = DateTime.Today.AddDays(1);

            var practiceViewModels = await practiceRepository
                 .Where(p => groupIds.Contains(p.GroupId))
                 .Where(p => p.State == Status.Public)
                 .Where(p => p.BeginDateTime < tomorrow && p.EndDateTime >= today)
                 .OrderBy(p => p.BeginDateTime)
                 .ProjectTo<PracticeViewModel>()
                 .ToListAsync();

            var viewModel = new MyPageIndexViewModel()
            {
                Groups = groups,
                PracticeViewModels = practiceViewModels
            };

            return viewModel;
        }
    }
}
