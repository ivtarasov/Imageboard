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
            var post = await _context.Posts.AsNoTracking().Include(p => p.Tread)
                                                              .ThenInclude(t => t.Board)
                                                          .FirstOrDefaultAsync(p => p.Tread.Board.Name == boardName && p.Id == postId);

            if (post == null) return (false, 0);

            return (true, post.TreadId);
        }

        public async Task<string> GetBoardDescriptionAsync(string boardName) =>
           (await _context.Boards.AsNoTracking().FirstOrDefaultAsync(b => b.Name == boardName))?.Description;

        public async Task<Board> FindBoardAsync(string boardName) =>
            (await _context.Boards.AsNoTracking().FirstOrDefaultAsync(b => b.Name == boardName))?.ToModel();

        public async Task<List<string>> GetBoardNamesAsync() =>
            await _context.Boards.AsNoTracking().Select(b => b.Name).OrderBy(n => n).ToListAsync();

        public async Task<int> CountTreadsAsync(string boardName)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Name == boardName);

            return _context.Entry(board).Collection(b => b.Treads).Query().Take(100).Count();
        }

        public async Task<bool> TryAddBoardAsync(Board board)
        {
            _context.Boards.Add(board.ToEntety());

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

        public async Task<(bool, int)> TryAddTreadToBoardAsync(Tread tread, string boardName)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Name == boardName);
            if (board == null) return (false, 0);

            TreadEntety treadEntety = tread.ToEntety();
            treadEntety.Board = board;

            _context.Treads.Add(treadEntety);

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

            _context.Posts.Add(postEntety);

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

        public async Task<Board> FindAndLoadBoardAsync(string boardName, int page, int pageSize)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Name == boardName);
            if (board == null) return null;

            LoadBoard(board, page, pageSize);
            return board.ToModel();
        }

        private void LoadBoard(BoardEntety board, int page, int pageSize)
        {
            _context.Entry(board).Collection(b => b.Treads)
                                 .Query()
                                 .Include(t => t.Posts)
                                    .ThenInclude(p => p.Image)
                                 .OrderByDescending(t => t.Posts.OrderBy(p => p.Time).Take(500).LastOrDefault(p => !p.IsSage || p.IsOp).Time)
                                 .Skip((page - 1) * pageSize).Take(pageSize)
                                 .Load();
        }

        public async Task<Tread> FindAndLoadTreadAsync(string boardName, int treadId)
        {
            var tread = await _context.Treads.FirstOrDefaultAsync(t => t.Board.Name == boardName && t.Id == treadId);
            if (tread == null) return null;

            LoadTread(tread);
            return tread.ToModel();
        }

        private void LoadTread(TreadEntety tread)
        {
            _context.Entry(tread).Collection(t => t.Posts)
                                 .Query()
                                 .Include(p => p.Image)
                                 .OrderBy(p => p.Time)
                                 .Load();
        }

        public async Task<bool> TryDeleteBoardAsync(Board board)
        {
            var boardEntety = board.ToEntety();
            _context.Attach(boardEntety);
            _context.Remove(boardEntety);

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

        public async Task<bool> TryDeleteAsync(IEnumerable<int> postIds, string ip, string password, bool isTreadDeletionAllowed)
        {
            var posts = _context.Posts.Where(p => postIds.Contains(p.Id));
            var oPosts = posts.Where(p => p.IsOp);

            if (isTreadDeletionAllowed) DeleteTreads(oPosts);
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

        private void DeleteTreads(IEnumerable<PostEntety> oPosts)
        {
            var treadIds = oPosts.Select(p => p.TreadId);
            var treads = _context.Treads.Where(t => treadIds.Contains(t.Id));

            _context.Treads.RemoveRange(treads);
        }

        private void DeletePosts(IEnumerable<PostEntety> posts, string ip, string password) =>
            _context.Posts.RemoveRange(posts.Where(p => PassChecker.Check(p.PassHash, ip, password)));
    }
}
