using PostEntety = Netaba.Data.Enteties.Post;
using TreadEntety = Netaba.Data.Enteties.Tread;
using BoardEntety = Netaba.Data.Enteties.Board;
using Microsoft.EntityFrameworkCore;
using Netaba.Data.Contexts;
using Netaba.Data.Models;
using Netaba.Services.Mappers;
using Netaba.Services.Pass;
using System;
using System.Collections.Generic;
using System.Linq;

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
                _context.Boards.AddRange(TestDataGenerator.GenerateData());
                _context.SaveChanges();
            }
        }

        public bool TryGetPostLocation(int postId, string boardName, out int treadId)
        {
            treadId = 0;
            var board = _context.Boards.FirstOrDefault(b => b.Name == boardName);
            if (board == null) return false;

            var post = _context.Posts.Find(board.Id, postId);
            if (post == null) return false;

            _context.Entry(post).Reference(p => p.Tread);
            treadId = post.TreadId;
            return true;
        }

        public bool TryAddNewTreadToBoard(Tread tread, string boardName, out int treadId)
        {
            treadId = 0;
            var board = _context.Boards.FirstOrDefault(b => b.Name == boardName);
            if (board == null) return false;

            TreadEntety treadEntety = ModelMapper.ToEntety(tread);
            treadEntety.Board = board;

            _context.Treads.Add(treadEntety);
            _context.SaveChanges();

            treadId = treadEntety.Id;
            return true;
        }

        public bool TryAddNewPostToTread(Post post, string boardName, int treadId, out int postId)
        {
            postId = 0;
            var board = _context.Boards.FirstOrDefault(b => b.Name == boardName);
            if (board == null) return false;

            var tread = _context.Treads.Find(board.Id, treadId);
            if (tread == null) return false;

            PostEntety postEntety = ModelMapper.ToEntety(post);
            postEntety.Tread = tread;

            _context.Posts.Add(postEntety);
            _context.SaveChanges();

            postId = postEntety.Id;
            return true;
        }

        public Board FindAndLoadBoard(string boardName, int page, out int count)
        {
            count = 0;
            var board = _context.Boards.FirstOrDefault(b => b.Name == boardName);
            if (board == null) return null;
            else return LoadBoard(board, page, ref count);
        }

        public Board LoadBoard(BoardEntety board, int page, ref int count)
        {
            _context.Entry(board).Collection(b => b.Treads).Load();
            foreach (var tread in board.Treads) LoadTread(tread);

            var pageSize = 10;
            var treads = board.Treads.OrderByDescending(t => t.Posts.Take(500).LastOrDefault(p => !p.IsSage)?.Time ?? t.Posts.Single(p => p.IsOp).Time);

            count = treads.Take(100).Count();

            board.Treads = treads.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return EntetyMapper.ToModel(board);
        }

        public Tread FindAndLoadTread(string boardName, int treadId)
        {
            var board = _context.Boards.FirstOrDefault(b => b.Name == boardName);
            if (board == null) return null;
            
            var tread = _context.Treads.Find(board.Id, treadId);
            if (tread == null) return null;
            else return LoadTread(tread);
        }

        public Tread LoadTread(TreadEntety tread)
        {
            _context.Entry(tread).Collection(t => t.Posts).Load();

            foreach (var post in tread.Posts) _context.Entry(post).Reference(p => p.Image).Load();

            tread.Posts = tread.Posts.OrderBy(p => p.Time).ToList();

            return EntetyMapper.ToModel(tread);
        }

        public void Delete(IEnumerable<int> postIds, string ip, string password)
        {
            var posts = _context.Posts.Where(p => postIds.Contains(p.Id));
            var oPosts = posts.Where(p => p.IsOp);

            DeleteTreads(oPosts, ip, password);
            DeletePosts(posts.Except(oPosts), ip, password);

            _context.SaveChanges();
        }

        private void DeleteTreads(IEnumerable<PostEntety> oPosts, string ip, string password)
        {
            var treadIds = oPosts.Where(p => PassChecker.Check(p.PassHash, ip, password))
                                    .Select(p => p.TreadId);
            var treads = _context.Treads.Where(t => treadIds.Contains(t.Id));

            _context.Treads.RemoveRange(treads);
        }

        private void DeletePosts(IEnumerable<PostEntety> posts, string ip, string password)
        {
            _context.Posts.RemoveRange(posts.Where(p => PassChecker.Check(p.PassHash, ip, password))
                                                .Select(p => p));
        }
    }
}
