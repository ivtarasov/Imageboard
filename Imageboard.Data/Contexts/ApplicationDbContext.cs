using Microsoft.EntityFrameworkCore;
using Imageboard.Data.Enteties;

namespace Imageboard.Data.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Board> Boards { get; set; }
        public DbSet<Tread> Treads { get; set; }
        public DbSet<Post> Posts { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
