﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Netaba.Data.Enums;
using Netaba.Data.Models;
using Netaba.Services.Repository;
using Netaba.Services.Markup;
using Netaba.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netaba.Web.Controllers
{
    public class BoardController : Controller
    {
        private readonly IRepository _repository;
        private readonly IParser _parser;

        private readonly int PageSize = 10; // from config in future
        private readonly int NumberOfDisplayedPostsInBoardPage = 11; //
        
        public BoardController(IRepository repository, IParser parser)
        {
            _repository = repository;
            _parser = parser;
        }

        [HttpGet]
        [Route("/{boardName}", Name = "Board")]
        [Route("/{boardName}/{treadId}", Name = "Tread")]
        public async Task<IActionResult> CreatePost(string boardName, int? treadId, int? page = 1)
        {
            if (treadId == null) return await StartNewTreadAsync(boardName, page.Value);
            else return await ReplyToTreadAsync(boardName, treadId.Value);
        }

        [HttpPost]
        [Route("/CreatePost", Name = "CreatePost")]
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

            return View(await MapToCreatePostViewModelAsync(tread, boardName));
        }

        [NonAction]
        private async Task<IActionResult> ReplyToTreadAsync(Post post, string boardName, int treadId, Destination dest)
        {
            if (!ModelState.IsValid)
            {
                var tread = await _repository.FindAndLoadTreadAsync(boardName, treadId);
                if (tread == null) return NotFound();

                return View(await MapToCreatePostViewModelAsync(tread, boardName));
            }

            post.Message = await _parser.ToHtmlAsync(post.Message, boardName);
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

            return View(MapToCreatePostViewModel(board, page));
        }

        [NonAction]
        private async Task<IActionResult> StartNewTreadAsync(Post post, string boardName, Destination dest)
        {
            if (!ModelState.IsValid)
            {
                var board = await _repository.FindAndLoadBoardAsync(boardName);
                if (board == null) return NotFound();

                return View(MapToCreatePostViewModel(board));
            }

            post.Message = await _parser.ToHtmlAsync(post.Message, boardName);
            var tread = new Tread(new List<Post> { post });
            var (isSuccess, treadId) = await _repository.TryAddTreadToBoardAsync(tread, boardName);

            if (!isSuccess) return BadRequest();

            if (dest == Destination.Board) return RedirectToRoute("Board", new { boardName });
            else return RedirectToRoute("Tread", new { boardName, treadId });
        }

        [NonAction]
        private async Task<CreatePostViewModel> MapToCreatePostViewModelAsync(Tread tread, string boardName)
        {
            var boardDescription = await _repository.GetBoardDescriptionAsync(boardName);
            var treadViewModel = new TreadViewModel(tread.Posts.Select((p, i) => new PostViewModel(p, ++i, false)).ToList(), tread.Id);
            return new CreatePostViewModel(new List<TreadViewModel> { treadViewModel }, boardName, boardDescription, tread.Id);
        }

        [NonAction]
        private CreatePostViewModel MapToCreatePostViewModel(Board board, int page = 1)
        {
            var count = board.Treads.Count;
            var pageViewModel = new PageViewModel(count, page, PageSize, board.Name);

            var treads = board.Treads.Skip((page - 1) * PageSize).Take(PageSize).ToList();
            var treadViewModels = treads.Select(t => new TreadViewModel(t.Posts.Select((p, i) => new PostViewModel(p, ++i, true)).ToList(), NumberOfDisplayedPostsInBoardPage, t.Id)).ToList();
            return new CreatePostViewModel(treadViewModels, board.Name, board.Description, pageViewModel);
        }
    }
}