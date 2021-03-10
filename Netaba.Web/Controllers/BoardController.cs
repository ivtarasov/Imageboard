using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Netaba.Data.Enums;
using Netaba.Data.Models;
using Netaba.Services.Repository;
using Netaba.Services.Markup;
using Netaba.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Netaba.Web.Controllers
{
    public class BoardController : Controller
    {
        private readonly IRepository _repository;
        private readonly IParser _parser;

        public BoardController(IRepository repository, IParser parser)
        {
            _repository = repository;
            _parser = parser;
        }

        [HttpGet]
        [Route("/{boardName}", Name = "Board")]
        [Route("/{boardName}/{treadId}", Name = "Tread")]
        public IActionResult CreatePost(string boardName, int? treadId, int? page = 1)
        {
            if (treadId == null) return StartNewTread(boardName, page.Value);
            else return ReplyToTread(boardName, treadId.Value);
        }

        [HttpPost]
        [Route("/CreatePost", Name = "CreatePost")]
        public IActionResult CreatePost(Post post, string boardName, int? treadId, Destination dest)
        {
            if (post == null) throw new NullReferenceException("The received Post instance was null.");
            if (post.IsOp) return StartNewTread(post, boardName, dest);
            else return ReplyToTread(post, boardName, treadId.Value, dest);
        }

        [HttpPost]
        [Route("/Delete", Name = "Delete")]
        public IActionResult DeletePosts(Dictionary<int, int> ids, string boardName, string password)
        {
            if (ids == null) return RedirectToRoute("Board", new { boardName });

            _repository.Delete(ids.Values, HttpContext.Connection.RemoteIpAddress.ToString(), password);
            return RedirectToRoute("Board", new { boardName });
        }

        [NonAction]
        public IActionResult ReplyToTread(string boardName, int treadId)
        {
            var tread = _repository.FindAndLoadTread(boardName, treadId);
            if (tread == null) return NotFound();
            else
            {
                var treadViewModel = new TreadViewModel(tread.Posts.Select((p, i) => new PostViewModel(p, ++i, false)).ToList(), treadId);
                return View(new CreatePostViewModel(new List<TreadViewModel> { treadViewModel }, ReplyFormAction.ReplyToTread, boardName, treadId));
            }
        }

        [NonAction]
        public IActionResult ReplyToTread(Post post, string boardName, int treadId, Destination dest)
        {
            if (!ModelState.IsValid)
            {
                var tread = _repository.FindAndLoadTread(boardName, treadId);
                if (tread == null) return NotFound();
                var treadViewModel = new TreadViewModel(tread.Posts.Select((p, i) => new PostViewModel(p, ++i, false)).ToList(), treadId);
                return View(new CreatePostViewModel(new List<TreadViewModel>{ treadViewModel }, ReplyFormAction.ReplyToTread, post, boardName, treadId));
            }

            post.Message = _parser.ToHtml(post.Message, boardName);
            if (!_repository.TryAddNewPostToTread(post, boardName, treadId, out int postId)) return NotFound();

            if (dest == Destination.Tread) return RedirectToRoute("Tread", new { boardName, treadId});
            else return RedirectToRoute("Board", new { boardName });
        }

        [NonAction]
        public IActionResult StartNewTread(string boardName, int page)
        {
            var pageSize = 10;
            var board = _repository.FindAndLoadBoard(boardName, page, out int count);
            if (board == null) return NotFound();

            var pageViewModel = new PageViewModel(count, page, pageSize, boardName);

            var treadViewModels = board.Treads.Select(t => new TreadViewModel(t.Posts.Select((p, i) => new PostViewModel(p, ++i, true)).ToList(), 11, t.Id)).ToList();
            return View(new CreatePostViewModel(treadViewModels, ReplyFormAction.StartNewTread, boardName, pageViewModel));
        }

        [NonAction]
        public IActionResult StartNewTread(Post post, string boardName, Destination dest)
        {
            if (!ModelState.IsValid)
            {
                var page = 1;
                var pageSize = 10;
                var board = _repository.FindAndLoadBoard(boardName, page, out int count);
                if (board == null) return NotFound();

                var pageViewModel = new PageViewModel(count, page, pageSize, boardName);

                var treadViewModels = board.Treads.Select(t => new TreadViewModel(t.Posts.Select((p, i) => new PostViewModel(p, ++i, true)).ToList(), 11, t.Id)).ToList();
                return View(new CreatePostViewModel(treadViewModels, ReplyFormAction.StartNewTread, post, boardName, pageViewModel));
            }

            post.Message = _parser.ToHtml(post.Message, boardName);
            var tread = new Tread(new List<Post> { post });
            if (!_repository.TryAddNewTreadToBoard(tread, boardName, out int treadId)) return NotFound();

            if (dest == Destination.Board) return RedirectToRoute("Board", new { boardName });
            else return RedirectToRoute("Tread", new { boardName, treadId });
        }
    }
}
