using System;
using System.Collections.Generic;
using System.Linq;
using Netaba.Data.Contexts;
using Netaba.Data.Enteties;
using Microsoft.EntityFrameworkCore;
using Netaba.Services.Pass;

namespace Netaba.Services.Repository
{
    public class Repository: IRepository
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;

            if (!_context.Boards.Any())
            {
                _context.Boards.Add(TestDataGenerator.GenerateData());
                _context.SaveChanges();
            }
        }

        public bool IsThereBoard(int boardId) => _context.Boards.Find(boardId) != null;

        public bool TryFindPost(int postId, out (int BoardId, int TreadId) postPlace)
        {
            var post = _context.Posts.Find(postId);
            if (post == null)
            {
                postPlace = (0, 0);
                return false;
            }

            _context.Entry(post).Reference(p => p.Tread);
            postPlace = (post.Tread.BoardId, post.TreadId);
            return true;
        }

        public void DeletePosts(IEnumerable<int> postIds, string ip, string password)
        {
            var posts = _context.Posts.Where(p => postIds.Contains(p.Id));
            var oPosts = posts.Where(p => p.IsOp);

            DeleteTreads(oPosts, ip, password);
            DeletePosts(posts.Except(oPosts), ip, password);

            _context.SaveChanges();
        }

        private void DeleteTreads(IEnumerable<Post> oPosts, string ip, string password)
        {
            var treadIds = oPosts.Where(p => PassChecker.Check(p, ip, password)).Select(p => p.TreadId);
            var treads = _context.Treads.Where(t => treadIds.Contains(t.Id));

            _context.Treads.RemoveRange(treads);
        }

        private void DeletePosts(IEnumerable<Post> posts, string ip, string password)
        {
            _context.Posts.RemoveRange(posts.Where(p => PassChecker.Check(p, ip, password)));
        }

        public void AddNewTreadToBoard(Tread tread, int boardId)
        {
            Board board = _context.Boards.Single(t => t.Id == boardId);
            board.Treads.Add(tread);

            _context.SaveChanges();
        }

        public void AddNewPost(Post post, int treadId)
        {
            Tread tread = _context.Treads.Single(t => t.Id == treadId);
            tread.Posts.Add(post);

            _context.SaveChanges();
        }

        public Board LoadBoard(int boardId)
        {
            var board = _context.Boards.FirstOrDefault(b => b.Id == boardId);
            if (board == null) return null;

            _context.Entry(board).Collection(b => b.Treads).Load();
            foreach (var tread in board.Treads) LoadTread(tread.Id);

            board.Treads = board.Treads.OrderByDescending(t => t.Posts.Take(500).LastOrDefault(p => !p.IsSage)?.Time ?? t.Posts.Single(p => p.IsOp).Time).ToList();

            return board;
        }

        public Tread LoadTread(int treadId)
        {
            var tread = _context.Treads.FirstOrDefault(t => t.Id == treadId);
            if (tread == null) return null;

            _context.Entry(tread).Collection(t => t.Posts).Load();
            foreach (var post in tread.Posts) _context.Entry(post).Reference(p => p.Image).Load();

            tread.Posts = tread.Posts.OrderBy(p => p.Time).ToList();

            return tread;
        }
    }
}
