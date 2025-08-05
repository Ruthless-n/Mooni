using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class _dbContext : DbContext
    {
        public _dbContext(DbContextOptions<_dbContext> options) : base(options) { 
        }

        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().ToTable("Users", "public").HasKey(u => u.Id);
        }
    }
}
