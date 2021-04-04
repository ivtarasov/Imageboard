using Microsoft.EntityFrameworkCore;
using Netaba.Data.Enteties;

namespace Netaba.Data.Contexts
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}
