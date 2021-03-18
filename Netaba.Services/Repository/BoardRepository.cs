using Microsoft.EntityFrameworkCore;
using Netaba.Data.Contexts;
using Netaba.Data.Models;
using Netaba.Services.Mappers;
using Netaba.Services.Pass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardEntety = Netaba.Data.Enteties.Board;
using PostEntety = Netaba.Data.Enteties.Post;
using TreadEntety = Netaba.Data.Enteties.Tread;

namespace Netaba.Services.Repository
{
    public class BoardRepository: IBoardRepository
    {
        private readonly BoardDbContext _context;
        public BoardRepository(BoardDbContext context)
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
            var post = await _context.Posts.Include(p => p.Tread)
                                           .ThenInclude(t => t.Board)
                                           .FirstOrDefaultAsync(p => p.Tread.Board.Name == boardName && p.Id == postId);

            if (post == null) return (false, 0);

            return (true, post.TreadId);
        }

        public async Task<string> GetBoardDescriptionAsync(string boardName) =>
           (await _context.Boards.FirstOrDefaultAsync(b => b.Name == boardName))?.Description;

        public async Task<(bool, int)> TryAddTreadToBoardAsync(Tread tread, string boardName)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Name == boardName);
            if (board == null) return (false, 0);

            TreadEntety treadEntety = tread.ToEntety();
            treadEntety.Board = board;

            await _context.Treads.AddAsync(treadEntety);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return (false, 0);
            }

            return (true, treadEntety.Id);
        }

        public async Task<(bool, int)> TryAddPostToTreadAsync(Post post, string boardName, int treadId)
        {
            var tread = await _context.Treads.Include(t => t.Board)
                                             .FirstOrDefaultAsync(t => t.Board.Name == boardName && t.Id == treadId);

            if (tread == null) return (false, 0);

            PostEntety postEntety = post.ToEntety();
            postEntety.Tread = tread;

            await _context.Posts.AddAsync(postEntety);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return (false, 0);
            }

            return (true, postEntety.Id);
        }

        public async Task<Board> FindAndLoadBoardAsync(string boardName)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Name == boardName);
            if (board == null) return null;

            LoadBoard(board);
            return board.ToModel();
        }

        private void LoadBoard(BoardEntety board)
        {
            _context.Entry(board).Collection(b => b.Treads).Load();

            foreach (var tread in board.Treads) LoadTread(tread);

            board.Treads = board.Treads.OrderByDescending(t => t.Posts.Take(500).LastOrDefault(p => !p.IsSage)?.Time ?? t.Posts.FirstOrDefault(p => p.IsOp).Time)
                                       .Take(100)
                                       .ToList();
        }

        public async Task<Tread> FindAndLoadTreadAsync(string boardName, int treadId)
        {
            var tread = await _context.Treads.Include(t => t.Board)
                                             .FirstOrDefaultAsync(t => t.Board.Name == boardName && t.Id == treadId);

            if (tread == null) return null;

            LoadTread(tread);
            return tread.ToModel();
        }

        private void LoadTread(TreadEntety tread)
        {
            _context.Entry(tread).Collection(t => t.Posts).Load();

            foreach (var post in tread.Posts) _context.Entry(post).Reference(p => p.Image).Load();

            tread.Posts = tread.Posts.OrderBy(p => p.Time).ToList();
        }

        public async Task<bool> TryDeleteAsync(IEnumerable<int> postIds, string ip, string password, bool isTreadDeletionAllowed)
        {
            var posts = _context.Posts.Where(p => postIds.Contains(p.Id));
            var oPosts = posts.Where(p => p.IsOp);

            if (isTreadDeletionAllowed) DeleteTreads(oPosts, ip, password);
            DeletePosts(posts.Except(oPosts), ip, password);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
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
