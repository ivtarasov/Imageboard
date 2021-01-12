using Microsoft.EntityFrameworkCore;

namespace Imageboard.Web.Models
{
    public class BoardsContext : DbContext
    {
        public DbSet<Board> Boards { get; set; }
        public BoardsContext(DbContextOptions<BoardsContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
