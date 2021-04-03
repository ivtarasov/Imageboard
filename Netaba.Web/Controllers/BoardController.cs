using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Netaba.Data.Enums;
using Netaba.Data.Models;
using Netaba.Data.ViewModels;
using Netaba.Services.Repository;
using Netaba.Services.Markup;
using Netaba.Services.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


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
        [Authorize(Roles = nameof(Role.SuperAdmin))]
        [Route("/add_board", Name = "BoardAdding")]
        public IActionResult AddBoard()
        {
            return View();
        }

        [HttpPost]
        [Route("/add_board", Name = "BoardAdding")]
        [Authorize(Roles = nameof(Role.SuperAdmin))]
        public async Task<IActionResult> AddBoard(Board board)
        {
            if (ModelState.IsValid)
            {
                var rboard = await _repository.FindBoardAsync(board.Name);
                if (rboard == null)
                {
                    bool isSuccess = await _repository.TryAddBoardAsync(board);
                    if (!isSuccess)
                    {
                        ModelState.AddModelError("", "Unable to add board.");
                        return View(new AddBoardViewModel(board));
                    }

                    return RedirectToRoute("BoardAdding");
                }
                else ModelState.AddModelError("", "Board with this name already exists.");
            }
            return View(new AddBoardViewModel(board));
        }

        [HttpGet]
        [Authorize(Roles = nameof(Role.SuperAdmin))]
        [Route("/del_board", Name = "BoardDeleting")]
        public IActionResult DeleteBoard()
        {
            return View();
        }

        [HttpPost]
        [Route("/del_board", Name = "BoardDeleting")]
        [Authorize(Roles = nameof(Role.SuperAdmin))]
        public async Task<IActionResult> DeleteBoard([Required(ErrorMessage = "Name is not specified.")] string boardName)
        {
            if (ModelState.IsValid)
            {
                var rboard = await _repository.FindBoardAsync(boardName);
                if (rboard != null)
                {
                    bool isSuccess = await _repository.TryDeleteBoardAsync(rboard);
                    if (!isSuccess)
                    {
                        ModelState.AddModelError("", "Unable to delete board.");
                        return View(new DeleteBoardViewModel(boardName));
                    }

                    return RedirectToRoute("BoardAdding");
                }
                else ModelState.AddModelError("", "Board with this name does not exist.");
            }
            return View(new DeleteBoardViewModel(boardName));
        }

        [HttpPost]
        [Route("/del_posts", Name = "PostDeleting")]
        public async Task<IActionResult> Delete([Required] Dictionary<int, int> ids, [Required] string boardName, string password)
        {
            if (!ModelState.IsValid)
            {
                if (boardName != null) return RedirectToRoute("Board", new { boardName });
                else return BadRequest();
            }

            //
            bool isAdminRequest = User.IsInRole(nameof(Role.Admin)) || User.IsInRole(nameof(Role.SuperAdmin));
            await _repository.TryDeleteAsync(ids.Values, HttpContext.Connection.RemoteIpAddress.ToString(), password, isAdminRequest);

            return RedirectToRoute("Board", new { boardName });
        }

        [HttpGet]
        [Route("/", Name = "MainPage")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/{boardName}", Name = "Board")]
        [Route("/{boardName}/{treadId}", Name = "Tread")]
        public async Task<IActionResult> CreatePost(string boardName, int? treadId, int? page = 1)
        {
            if (treadId == null) return await StartNewTread(boardName, page.Value);
            else return await ReplyToTread(boardName, treadId.Value);
        }

        [HttpPost]
        [Route("/{boardName}", Name = "Board")]
        [Route("/{boardName}/{treadId}", Name = "Tread")]
        public async Task<IActionResult> CreatePost([Required] Post post, [Required] string boardName, int? treadId, [Required] Destination dest)
        {
            if (post?.IsOp ?? true) return await StartNewTread(post, boardName, dest);
            else return await ReplyToTread(post, boardName, treadId.Value, dest);
        }

        [NonAction]
        private async Task<IActionResult> ReplyToTread(string boardName, int treadId)
        {
            var tread = await _repository.FindAndLoadTreadAsync(boardName, treadId);
            if (tread == null) return NotFound();

            return View(tread.ToCreatePostViewModel(boardName, await _repository.GetBoardDescriptionAsync(boardName)));
        }

        [NonAction]
        private async Task<IActionResult> ReplyToTread(Post post, string boardName, int treadId, Destination dest)
        {
            if (!ModelState.IsValid)
            {
                if (boardName == null) return BadRequest();

                var tread = await _repository.FindAndLoadTreadAsync(boardName, treadId);
                if (tread == null) return NotFound();

                return View(tread.ToCreatePostViewModel(boardName, await _repository.GetBoardDescriptionAsync(boardName), post));
            }

            await post.ParseMessageAsync(_parser, boardName);

            var (isSuccess, _) = await _repository.TryAddPostToTreadAsync(post, boardName, treadId);

            if (!isSuccess)
            {
                ModelState.AddModelError("", "Unable to create post.");

                var tread = await _repository.FindAndLoadTreadAsync(boardName, treadId);
                if (tread == null) return NotFound();

                return View(tread.ToCreatePostViewModel(boardName, await _repository.GetBoardDescriptionAsync(boardName), post));
            }

            if (dest == Destination.Tread) return RedirectToRoute("Tread", new { boardName, treadId});
            else return RedirectToRoute("Board", new { boardName });
        }

        [NonAction]
        private async Task<IActionResult> StartNewTread(string boardName, int page)
        {
            if (page <= 0) return BadRequest();

            var board = await _repository.FindAndLoadBoardAsync(boardName, page, PageSize);
            if (board == null) return NotFound();

            var count = await _repository.CountTreadsAsync(boardName);
            return View(board.ToCreatePostViewModel(PostsFromTreadOnBoardView, count, PageSize, page: page));
        }

        [NonAction]
        private async Task<IActionResult> StartNewTread(Post post, string boardName, Destination dest)
        {
            if (!ModelState.IsValid)
            {
                if (boardName == null) return BadRequest();

                var board = await _repository.FindAndLoadBoardAsync(boardName, 1, PageSize);
                if (board == null) return NotFound();

                var count = await _repository.CountTreadsAsync(boardName);
                return View(board.ToCreatePostViewModel(PostsFromTreadOnBoardView, count, PageSize, post: post));
            }

            await post.ParseMessageAsync(_parser, boardName);

            var tread = new Tread(new List<Post> { post });
            var (isSuccess, treadId) = await _repository.TryAddTreadToBoardAsync(tread, boardName);

            if (!isSuccess)
            {
                ModelState.AddModelError("", "Unable to create tread.");

                var board = await _repository.FindAndLoadBoardAsync(boardName, 1, PageSize);
                if (board == null) return NotFound();

                var count = await _repository.CountTreadsAsync(boardName);
                return View(board.ToCreatePostViewModel(PostsFromTreadOnBoardView, count, PageSize, post: post));
            }

            if (dest == Destination.Board) return RedirectToRoute("Board", new { boardName });
            else return RedirectToRoute("Tread", new { boardName, treadId });
        }
    }
}
