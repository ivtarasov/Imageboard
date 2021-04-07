using Microsoft.EntityFrameworkCore;
using Netaba.Data.Enteties;

namespace Netaba.Data.Contexts
{
    public class UsersDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options): base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();
        }
    }
}
