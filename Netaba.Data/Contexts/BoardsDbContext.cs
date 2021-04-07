using Microsoft.EntityFrameworkCore;
using Netaba.Data.Entities;

namespace Netaba.Data.Contexts
{
    public class BoardsDbContext : DbContext
    {
        public DbSet<Board> Boards { get; set; }
        public DbSet<Tread> Treads { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Image> Images { get; set; }

        public BoardsDbContext(DbContextOptions<BoardsDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>()
                .ToTable("Boards");

            modelBuilder.Entity<Board>()
                .HasIndex(b => b.Name)
                .IsUnique();

            modelBuilder.Entity<Board>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Tread>()
                .ToTable("Treads");

            modelBuilder.Entity<Tread>()
                .HasKey(t => new { t.BoardId, t.Id });

            modelBuilder.Entity<Tread>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Tread>()
                .HasOne(p => p.Board)
                .WithMany(b => b.Treads)
                .HasForeignKey(t => t.BoardId);

            modelBuilder.Entity<Post>()
                .ToTable("Posts");

            modelBuilder.Entity<Post>()
                .HasKey(p => new { p.BoardId, p.Id });

            modelBuilder.Entity<Post>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Tread)
                .WithMany(t => t.Posts)
                .HasForeignKey(p => new { p.BoardId, p.TreadId })
                .HasPrincipalKey(t => new { t.BoardId, t.Id });

            modelBuilder.Entity<Image>()
                .ToTable("Images");
        }
    }
}