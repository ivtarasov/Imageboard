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
using System.Security.Claims;

namespace Netaba.Services.Repository
{
    public class BoardRepository: IBoardRepository
    {
        private readonly BoardContext _context;
        public BoardRepository(BoardContext context)
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

            var post = await _context.Posts.FirstOrDefaultAsync(p => p.BoardId == board.Id && p.Id == postId);
            if (post == null) return (false, 0);

            return (true, post.TreadId);
        }

        public async Task<string> GetBoardDescriptionAsync(string boardName) =>
           (await _context.Boards.FirstOrDefaultAsync(b => b.Name == boardName))?.Description;

        public async Task<(bool, int)> TryAddTreadToBoardAsync(Tread tread, string boardName)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Name == boardName);
            if (board == null) return (false, 0);

            TreadEntety treadEntety = ModelMapper.ToEntety(tread);
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
            var board = _context.Boards.FirstOrDefault(b => b.Name == boardName);
            if (board == null) return (false, 0);

            var tread = await _context.Treads.FirstOrDefaultAsync(t => t.BoardId == board.Id && t.Id == treadId);
            if (tread == null) return (false, 0);

            PostEntety postEntety = ModelMapper.ToEntety(post);
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

            return EntetyMapper.ToModel(board);
        }

        private void LoadBoard(BoardEntety board)
        {
            _context.Entry(board).Collection(b => b.Treads).Load();

            foreach (var tread in board.Treads) LoadTread(tread);

            board.Treads = board.Treads.OrderByDescending(t => t.Posts.Take(500).LastOrDefault(p => !p.IsSage)?.Time ?? t.Posts.FirstOrDefault(p => p.IsOp).Time).Take(100).ToList();
        }

        public async Task<Tread> FindAndLoadTreadAsync(string boardName, int treadId)
        {
            var board = _context.Boards.FirstOrDefault(b => b.Name == boardName);
            if (board == null) return null;

            var tread = await _context.Treads.FirstOrDefaultAsync(t => t.BoardId == board.Id && t.Id == treadId);
            if (tread == null) return null;

            LoadTread(tread);

            return EntetyMapper.ToModel(tread);
        }

        private void LoadTread(TreadEntety tread)
        {
            _context.Entry(tread).Collection(t => t.Posts).Load();

            foreach (var post in tread.Posts) _context.Entry(post).Reference(p => p.Image).Load();

            tread.Posts = tread.Posts.OrderBy(p => p.Time).ToList();
        }

        public async Task<bool> TryDeleteAsync(IEnumerable<int> postIds, string ip, string password)
        {
            var posts = _context.Posts.Where(p => postIds.Contains(p.Id));
            var oPosts = posts.Where(p => p.IsOp);

            DeleteTreads(oPosts, ip, password);
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
