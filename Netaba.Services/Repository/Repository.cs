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
using System.Threading.Tasks;

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

        public async Task<(bool, int)> TryGetPostLocationAsync(int postId, string boardName)
        {
            var board = _context.Boards.FirstOrDefault(b => b.Name == boardName);
            if (board == null) return (false, 0);

            var post = await _context.Posts.FindAsync(board.Id, postId);
            if (post == null) return (false, 0);

            _context.Entry(post).Reference(p => p.Tread);
            return (true, post.TreadId);
        }

        public async Task<(bool, int)> TryAddTreadToBoardAsync(Tread tread, string boardName)
        {
            var board = _context.Boards.FirstOrDefault(b => b.Name == boardName);
            if (board == null) return (false, 0);

            TreadEntety treadEntety = ModelMapper.ToEntety(tread);
            treadEntety.Board = board;

            await _context.Treads.AddAsync(treadEntety);
            await _context.SaveChangesAsync();

            return (true, treadEntety.Id);
        }

        public async Task<(bool, int)> TryAddPostToTreadAsync(Post post, string boardName, int treadId)
        {
            var board = _context.Boards.FirstOrDefault(b => b.Name == boardName);
            if (board == null) return (false, 0);

            var tread = _context.Treads.Find(board.Id, treadId);
            if (tread == null) return (false, 0);

            PostEntety postEntety = ModelMapper.ToEntety(post);
            postEntety.Tread = tread;

            await _context.Posts.AddAsync(postEntety);
            await _context.SaveChangesAsync();

            return (true, postEntety.Id);
        }

        public async Task<(Board, int)> FindAndLoadBoardAsync(string boardName, int page)
        {
            var board = _context.Boards.FirstOrDefault(b => b.Name == boardName);

            if (board == null) return (null, 0);

            var count = await LoadBoardAsync(board, page);

            return (EntetyMapper.ToModel(board), count);
        }

        public async Task<int> LoadBoardAsync(BoardEntety board, int page)
        {
            _context.Entry(board).Collection(b => b.Treads).Load();

            await Task.WhenAll(board.Treads.Select(t =>  LoadTreadAsync(t)));

            var pageSize = 10;
            var treads = board.Treads.OrderByDescending(t => t.Posts.Take(500).LastOrDefault(p => !p.IsSage)?.Time ?? t.Posts.FirstOrDefault(p => p.IsOp).Time)
                                     .Take(100);

            var count = treads.Count();

            board.Treads = treads.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return count;
        }

        public async Task<Tread> FindAndLoadTreadAsync(string boardName, int treadId)
        {
            var board = _context.Boards.FirstOrDefault(b => b.Name == boardName);
            if (board == null) return null;
            
            var tread = await _context.Treads.FindAsync(board.Id, treadId);
            if (tread == null) return null;

            await LoadTreadAsync(tread);

            return EntetyMapper.ToModel(tread);
        }

        public async Task LoadTreadAsync(TreadEntety tread)
        {
            _context.Entry(tread).Collection(t => t.Posts).Load();

            await Task.WhenAll(tread.Posts.Select(p => _context.Entry(p).Reference(p => p.Image).LoadAsync()));

            tread.Posts = tread.Posts.OrderBy(p => p.Time).ToList();
        }

        public async Task DeleteAsync(IEnumerable<int> postIds, string ip, string password)
        {
            var posts = _context.Posts.Where(p => postIds.Contains(p.Id));
            var oPosts = posts.Where(p => p.IsOp);

            DeleteTreads(oPosts, ip, password);
            DeletePosts(posts.Except(oPosts), ip, password);

            await _context.SaveChangesAsync();
        }

        private void DeleteTreads(IEnumerable<PostEntety> oPosts, string ip, string password)
        {
            var treadIds = oPosts.Where(p => PassChecker.Check(p.PassHash, ip, password)).Select(p => p.TreadId);

            var treads = _context.Treads.Where(t => treadIds.Contains(t.Id));

            _context.Treads.RemoveRange(treads);
        }

        private void DeletePosts(IEnumerable<PostEntety> posts, string ip, string password) =>
            _context.Posts.RemoveRange(posts.Where(p => PassChecker.Check(p.PassHash, ip, password)));
    }
}
