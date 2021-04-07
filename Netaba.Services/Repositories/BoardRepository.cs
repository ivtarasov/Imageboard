using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netaba.Data.Contexts;
using Netaba.Data.Models;
using Netaba.Data.Services.Hashing;
using Netaba.Services.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardEntity = Netaba.Data.Entities.Board;
using PostEntity = Netaba.Data.Entities.Post;
using TreadEntity = Netaba.Data.Entities.Tread;

namespace Netaba.Services.Repository
{
    public class BoardRepository: IBoardRepository
    {
        private readonly BoardsDbContext _context;
        private readonly ILogger _logger;
        public BoardRepository(BoardsDbContext context, ILogger<BoardRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<(bool, int)> TryGetPostLocationAsync(int postId, string boardName)
        {
            var post = await _context.Posts.AsNoTracking().Include(p => p.Tread)
                                                              .ThenInclude(t => t.Board)
                                                          .FirstOrDefaultAsync(p => p.Tread.Board.Name == boardName && p.Id == postId);

            if (post == null) return (false, 0);

            return (true, post.TreadId);
        }

        public async Task<string> GetBoardDescriptionAsync(string boardName)
        {
            var board = await _context.Boards.AsNoTracking().FirstOrDefaultAsync(b => b.Name == boardName);
            return board?.Description;
        }

        public async Task<Board> FindBoardAsync(string boardName)
        {
            var board = await _context.Boards.AsNoTracking().FirstOrDefaultAsync(b => b.Name == boardName);
            return board?.ToModel();
        }

        public async Task<List<string>> GetBoardNamesAsync()
        {
            return await _context.Boards.AsNoTracking().Select(b => b.Name).OrderBy(n => n).ToListAsync();
        }

        public async Task<int> CountTreadsAsync(string boardName)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Name == boardName);

            return _context.Entry(board).Collection(b => b.Treads).Query().AsNoTracking().Take(100).Count();
        }

        public async Task<bool> TryAddBoardAsync(Board board)
        {
            _context.Boards.Add(board.ToEntity());

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes in the TryAddBoardAsync method.");
                return false;
            }

            return true;
        }

        public async Task<(bool, int)> TryAddTreadToBoardAsync(Tread tread, string boardName)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Name == boardName);
            if (board == null) return (false, 0);

            TreadEntity treadEntity = tread.ToEntity();
            treadEntity.Board = board;
            treadEntity.TimeOfLastPost = treadEntity.Posts.First().Time;

            _context.Treads.Add(treadEntity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes in the TryAddTreadToBoardAsync method.");
                return (false, 0);
            }

            return (true, treadEntity.Id);
        }

        public async Task<(bool, int)> TryAddPostToTreadAsync(Post post, string boardName, int treadId)
        {
            var tread = await _context.Treads.Include(t => t.Board)
                                             .FirstOrDefaultAsync(t => t.Board.Name == boardName && t.Id == treadId);

            if (tread == null) return (false, 0);

            PostEntity postEntity = post.ToEntity();
            postEntity.Tread = tread;
            
            if (!postEntity.IsSage)
            {
                int count = _context.Entry(tread).Collection(t => t.Posts).Query().Count();
                if (count < 500) tread.TimeOfLastPost = postEntity.Time;
            }

            _context.Posts.Add(postEntity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes in the TryAddPostToTreadAsync method.");
                return (false, 0);
            }

            return (true, postEntity.Id);
        }

        public async Task<Board> FindAndLoadBoardAsync(string boardName, int page, int pageSize)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Name == boardName);

            if (board == null) return null;

            LoadBoard(board, page, pageSize);
            return board.ToModel();
        }

        private void LoadBoard(BoardEntity board, int page, int pageSize)
        {
            _context.Entry(board).Collection(b => b.Treads)
                                 .Query()
                                 .OrderByDescending(t => t.TimeOfLastPost)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .Load();

            foreach (var tread in board.Treads) LoadTread(tread);
        }

        public async Task<Tread> FindAndLoadTreadAsync(string boardName, int treadId)
        {
            var tread = await _context.Treads.Include(t => t.Board)
                                             .FirstOrDefaultAsync(t => t.Board.Name == boardName && t.Id == treadId);
            if (tread == null) return null;

            LoadTread(tread);
            return tread.ToModel();
        }

        private void LoadTread(TreadEntity tread)
        {
            _context.Entry(tread).Collection(t => t.Posts)
                                 .Query()
                                 .Include(p => p.Image)
                                 .OrderBy(p => p.Time)
                                 .Load();
        }

        public async Task<bool> TryDeleteBoardAsync(Board board)
        {
            var boardEntity = board.ToEntity();
            _context.Attach(boardEntity);
            _context.Remove(boardEntity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes in the TryDeleteBoardAsync method.");
                return false;
            }

            return true;
        }

        public async Task<bool> TryDeletePostsAndTreadsAsync(IEnumerable<int> postIds, string ip, string password, bool isTreadDeletionAllowed)
        {
            var posts = _context.Posts.Where(p => postIds.Contains(p.Id));
            var oPosts = posts.Where(p => p.IsOp);

            if (isTreadDeletionAllowed) DeleteTreads(oPosts);
            DeletePosts(posts.Except(oPosts), ip, password);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes in the TryDeleteAsync method.");
                return false;
            }

            return true;
        }

        private void DeleteTreads(IEnumerable<PostEntity> oPosts)
        {
            var treadIds = oPosts.Select(p => p.TreadId);
            var treads = _context.Treads.Where(t => treadIds.Contains(t.Id));

            _context.Treads.RemoveRange(treads);
        }

        private void DeletePosts(IEnumerable<PostEntity> posts, string ip, string password)
        {
            _context.Posts.RemoveRange(posts.Where(p => PassChecker.Check(p.PassHash, ip, password)));
        }
    }
}
