using AppMvc6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Services
{
    public interface IUnitOfWork
    {
        void Commit();
        Task CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Commit()
        {
            dbContext.Commit();
        }
        public async Task CommitAsync()
        {
            await dbContext.CommitAsync();
        }
    }
}
