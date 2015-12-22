using AppMvc6.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<ApplicationUser>> GetUsers();
        Task Update();
    }

    public class GroupService : IGroupService
    {
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly IUnitOfWork unitOfWork;
        public GroupService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            userRepository = unitOfWork.Repository<ApplicationUser>();
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            //return await userRepository.ToListAsync();
            return await userRepository.Where(u => u.UserName != "").ToListAsync();
        }

        public async Task Update()
        {
            var user = await userRepository.GetAsync(u => u.Id == 1);
            user.PhoneNumber = DateTime.Now.ToString();
            userRepository.Update(user);

            var user2 = await userRepository.GetAsync(u => u.Id == 2);
            user2.PhoneNumber = "--";
            userRepository.Update(user);

            await unitOfWork.CommitAsync();
        }
    }
}
