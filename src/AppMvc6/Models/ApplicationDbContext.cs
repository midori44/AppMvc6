using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace AppMvc6.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Enquete> Enquetes { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Practice> Practices { get; set; }
        public DbSet<ProfileIcon> ProfileIcons { get; set; }
        public DbSet<Song> Songs { get; set; }

        public DbSet<SongPractice> SongPractice { get; set; }


        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Group>().HasIndex(g => g.Account).IsUnique();
            

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public virtual void Commit()
        {
            SetModified();
            base.SaveChanges();
        }
        public virtual async Task CommitAsync()
        {
            SetModified();
            await base.SaveChangesAsync();
        }

        // 更新日時(Modified)の自動更新
        private void SetModified()
        {
            var now = DateTime.Now;
            var entities = ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified)
                    && e.Metadata.GetProperties().Any(p => p.Name == "Modified"))
                .Select(e => e.Entity);

            foreach (dynamic entity in entities)
            {
                entity.Modified = now;
            }
        }
    }
}
