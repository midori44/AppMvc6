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
        void Update();
    }

    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        public GroupService(ApplicationDbContext dbContext, IUnitOfWork unitOfWork)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            return await dbContext.Users.ToListAsync();
        }

        public void Update()
        {
            var user = dbContext.Users.FirstOrDefault();
            user.PhoneNumber = DateTime.Now.ToString();
            dbContext.Attach(user);
            dbContext.Entry(user).State = EntityState.Modified;

            var user2 = dbContext.Users.LastOrDefault();
            user2.PhoneNumber = "--";
            dbContext.Attach(user2);
            dbContext.Entry(user2).State = EntityState.Modified;


            unitOfWork.Commit();
        }
    }
}
