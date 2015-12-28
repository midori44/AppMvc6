using AppMvc6.Infrastructure;
using AppMvc6.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using MR.AspNet.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Services
{
    public interface IGroupService
    {
        Task<IPagedList<Group>> GetGroupsPageAsync(Page page);
        Task<IEnumerable<Group>> GetGroupsAsync(int userId);

        IEnumerable<Group> GetGroups(int userId, int num);
        IPagedList<Group> GetGroupsPage(Page page, int userId);

    }

    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork unitOfWork;
        //private readonly IRepository<ApplicationUser> userRepository;
        private readonly IRepository<Group> groupRepository;
        private readonly IRepository<Member> memberRepository;
        public GroupService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            //userRepository = unitOfWork.Repository<ApplicationUser>();
            groupRepository = unitOfWork.Repository<Group>();
            memberRepository = unitOfWork.Repository<Member>();
        }

        //public async Task<IEnumerable<ApplicationUser>> GetUsers()
        //{
        //    //return await userRepository.ToListAsync();
        //    return await userRepository.Where(u => u.UserName != "").ToListAsync();
        //}

        //public async Task Update()
        //{
        //    var user = await userRepository.GetAsync(u => u.Id == 1);
        //    user.PhoneNumber = DateTime.Now.ToString();
        //    userRepository.Update(user);

        //    var user2 = await userRepository.GetAsync(u => u.Id == 2);
        //    user2.PhoneNumber = "--";
        //    userRepository.Update(user);

        //    await unitOfWork.CommitAsync();
        //}

        public async Task<IPagedList<Group>> GetGroupsPageAsync(Page page)
        {
            var groups = groupRepository
                .Where(g => g.State == Status.Public)
                .OrderByDescending(g => g.Created);

            return await groups.ToPagedListAsync(page);
        }

        public IEnumerable<Group> GetGroups(int userId, int num)
        {
            throw new NotImplementedException();
        }

        public IPagedList<Group> GetGroupsPage(Page page, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Group>> GetGroupsAsync(int userId)
        {
            var groups = memberRepository
                .Where(m => m.UserId == userId && m.State == Status.Public)
                .OrderByDescending(m => m.Created)
                .Select(m => m.Group)
                .Where(g => g.State == Status.Public);

            return await groups.ToListAsync();
        }
    }
}
