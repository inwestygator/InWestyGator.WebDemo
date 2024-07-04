using InWestyGator.WebDemo.Core.Models;
using InWestyGator.WebDemo.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace InWestyGator.WebDemo.DataAccess
{
    public class WebDbContext : DbContext
    {
        public DbSet<Pack> Packs { get; set; }

        public WebDbContext(DbContextOptions<WebDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PackConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
