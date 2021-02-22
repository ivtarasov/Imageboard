using System;
using System.Collections.Generic;
using System.Linq;
using Imageboard.Data.Contexts;
using Imageboard.Data.Enteties;
using Microsoft.EntityFrameworkCore;

namespace Imageboard.Services.Repository
{
    public class Repository: IRepository
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            if (!_context.Boards.Any())
            {
                _context.Boards.Add(TestDataGenerator.GenerateData());
                _context.SaveChanges();
            }
        }

        public void Delete(IEnumerable<int> postIds)
        {
            var posts = _context.Posts.Where(p => postIds.Contains(p.Id));
            var oPosts = posts.Where(p => p.IsOp);

            DeleteTreads(oPosts);
            DeletePosts(posts.Except(oPosts));

            _context.SaveChanges();
        }

        private void DeleteTreads(IEnumerable<Post> oPosts)
        {
            var treadIds = oPosts.Select(p => p.TreadId);
            var treads = _context.Treads.Where(t => treadIds.Contains(t.Id));

            _context.Treads.RemoveRange(treads);
        }

        private void DeletePosts(IEnumerable<Post> posts)
        {
            _context.Posts.RemoveRange(posts);
        }

        public void AddNewTread(Tread tread, int boardId)
        {
            Board board = _context.Boards.Single(t => t.Id == boardId);
            board.Treads.Add(tread);

            _context.Update(board);
            _context.SaveChanges();
        }

        public void AddNewPost(Post post, int treadId)
        {
            Tread tread = _context.Treads.Single(t => t.Id == treadId);
            tread.Posts.Add(post);

            _context.Update(tread);
            _context.SaveChanges();
        }

        public Board LoadBoard(int boardId)
        {
            var board = _context.Boards.Single(b => b.Id == boardId);

            _context.Entry(board).Collection(b => b.Treads).Load();
            foreach (var tread in board.Treads) LoadTread(tread.Id);

            board.Treads = board.Treads.OrderByDescending(t => t.Posts.LastOrDefault(p => !p.IsSage)?.Time ?? t.Posts.Single(p => p.IsOp).Time).ToList();

            return board;
        }

        public Tread LoadTread(int treadId)
        {
            var tread = _context.Treads.Single(t => t.Id == treadId);

            _context.Entry(tread).Collection(t => t.Posts).Load();
            foreach (var post in tread.Posts) _context.Entry(post).Reference(p => p.Image).Load();
            Console.WriteLine();

            tread.Posts = tread.Posts.OrderBy(p => p.Time).ToList();

            return tread;
        }
    }
}
