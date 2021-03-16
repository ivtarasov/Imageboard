using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Netaba.Data.Enums;
using Netaba.Data.Models;
using Netaba.Services.Repository;
using Netaba.Services.Markup;
using Netaba.Services.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netaba.Web.Controllers
{
    public class BoardController : Controller
    {
        private readonly IBoardRepository _repository;
        private readonly IParser _parser;

        private readonly int PageSize = 10; // from config in future
        private readonly int PostsFromTreadOnBoardView = 11; //
        
        public BoardController(IBoardRepository repository, IParser parser)
        {
            _repository = repository;
            _parser = parser;
        }

        [HttpGet]
        [Route("/{boardName}", Name = "Board")]
        [Route("/{boardName}/{treadId}", Name = "Tread")]
        public async Task<IActionResult> CreatePostAsync(string boardName, int? treadId, int? page = 1)
        {
            if (treadId == null) return await StartNewTreadAsync(boardName, page.Value);
            else return await ReplyToTreadAsync(boardName, treadId.Value);
        }

        [HttpPost]
        [Route("/{boardName}", Name = "Board")]
        [Route("/{boardName}/{treadId}", Name = "Tread")]
        public async Task<IActionResult> CreatePostAsync(Post post, string boardName, int? treadId, Destination dest)
        {
            if (post == null) return BadRequest(); // it means that something like "Request body too large." happened

            if (post.IsOp) return await StartNewTreadAsync(post, boardName, dest);
            else return await ReplyToTreadAsync(post, boardName, treadId.Value, dest);
        }

        [HttpPost]
        [Route("/Delete", Name = "Delete")]
        public async Task<IActionResult> DeletePostsAsync(Dictionary<int, int> ids, string boardName, string password)
        {
            if (ids == null) return RedirectToRoute("Board", new { boardName });
            
            bool isSuccess = await _repository.TryDeleteAsync(ids.Values, HttpContext.Connection.RemoteIpAddress.ToString(), password);
            if (!isSuccess) return BadRequest();

            return RedirectToRoute("Board", new { boardName });
        }

        [NonAction]
        private async Task<IActionResult> ReplyToTreadAsync(string boardName, int treadId)
        {
            var tread = await _repository.FindAndLoadTreadAsync(boardName, treadId);

            if (tread == null) return NotFound();

            return View(tread.ToCreatePostViewModel(boardName, await _repository.GetBoardDescriptionAsync(boardName)));
        }

        [NonAction]
        private async Task<IActionResult> ReplyToTreadAsync(Post post, string boardName, int treadId, Destination dest)
        {
            if (!ModelState.IsValid)
            {
                var tread = await _repository.FindAndLoadTreadAsync(boardName, treadId);
                if (tread == null) return NotFound();

                return View(tread.ToCreatePostViewModel(boardName, await _repository.GetBoardDescriptionAsync(boardName), post));
            }

            await post.ParseMessageAsync(_parser, boardName);

            var (isSuccess, _) = await _repository.TryAddPostToTreadAsync(post, boardName, treadId);

            if (!isSuccess) return BadRequest();

            if (dest == Destination.Tread) return RedirectToRoute("Tread", new { boardName, treadId});
            else return RedirectToRoute("Board", new { boardName });
        }

        [NonAction]
        private async Task<IActionResult> StartNewTreadAsync(string boardName, int page)
        {
            var board = await _repository.FindAndLoadBoardAsync(boardName);
            if (board == null) return NotFound();

            return View(board.ToCreatePostViewModel(PostsFromTreadOnBoardView, PageSize, page: page));
        }

        [NonAction]
        private async Task<IActionResult> StartNewTreadAsync(Post post, string boardName, Destination dest)
        {
            if (!ModelState.IsValid)
            {
                var board = await _repository.FindAndLoadBoardAsync(boardName);
                if (board == null) return NotFound();

                return View(board.ToCreatePostViewModel(PostsFromTreadOnBoardView, PageSize, post: post));
            }

            await post.ParseMessageAsync(_parser, boardName);

            var tread = new Tread(new List<Post> { post });
            var (isSuccess, treadId) = await _repository.TryAddTreadToBoardAsync(tread, boardName);

            if (!isSuccess) return BadRequest();

            if (dest == Destination.Board) return RedirectToRoute("Board", new { boardName });
            else return RedirectToRoute("Tread", new { boardName, treadId });
        }
    }
}
