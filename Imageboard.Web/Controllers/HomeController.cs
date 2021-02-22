using Imageboard.Data.Enteties;
using Imageboard.Data.Enums;
using Imageboard.Web.Models.ViewModels;
using Imageboard.Services.Markup;
using Imageboard.Services.ImageHandling;
using Imageboard.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Imageboard.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;
        private readonly IParser _parser;
        private readonly IImageHandler _imageHandler;
        private readonly IWebHostEnvironment _appEnvironment;
        
        public HomeController(IRepository repository, IParser parser, IImageHandler imageHandler, IWebHostEnvironment appEnvironment)
        {
            _repository = repository;
            _parser = parser;
            _imageHandler = imageHandler;
            _appEnvironment = appEnvironment;
        }

        [HttpPost]
        public IActionResult Delete(Dictionary<int, int> ids, int boardId)
        {
            _repository.Delete(ids.Values);
            return RedirectToAction("DisplayBoard", new { id = boardId });
        }

        [HttpPost]
        public IActionResult ReplyToTread(string message, string title, bool isSage, int treadId, IFormFile file, Destination dest)
        {
            Image img = _imageHandler.HandleImage(file, _appEnvironment.WebRootPath);
            tread.Posts.Add(new Post(_parser.ToHtml(message), title, DateTime.Now, img, false, isSage, tread));

            _repository.AddNewPost();

            if (dest == Destination.Tread) return RedirectToAction("DisplayTread", new { id = treadId });
            else return RedirectToAction("DisplayBoard", new { id = tread.BoardId });
        }

        [HttpPost]
        public IActionResult StartNewTread(string message, string title, bool isSage, int boardId, IFormFile file, Destination dest)
        {
            var board = _db.Boards.Single(t => t.Id == boardId);

            Image img = _imageHandler.HandleImage(file, _appEnvironment.WebRootPath);
            var oPost = new Post(_parser.ToHtml(message, _db), title, DateTime.Now, img, true, isSage);
            var tread = new Tread(board, oPost);

            board.Treads.Add(tread);

            _db.Update(board);
            _db.SaveChanges();

            if (dest == Destination.Board) return RedirectToAction("DisplayBoard", new { id = boardId });
            else return RedirectToAction("DisplayTread", new { id = tread.Id });
        }

        [HttpGet]
        public IActionResult DisplayBoard(int id)
        {
            var board = _db.Boards.Single(b => b.Id == id);

            _db.Entry(board).Collection(b => b.Treads).Load();

            foreach (var tread in board.Treads)
            {
                _db.Entry(tread).Collection(t => t.Posts).Load();
                foreach (var post in tread.Posts) _db.Entry(post).Reference(p => p.Image).Load();

                tread.Posts = tread.Posts.OrderBy(p => p.Time).ToList();
            }

            board.Treads = board.Treads.OrderByDescending(t => t.Posts.LastOrDefault(p => !p.IsSage)?.Time ?? t.Posts.Single(p => p.IsOp).Time).ToList();

            return View(new BoardViewModel(board));
        }

        [HttpGet]
        public IActionResult DisplayTread(int id)
        {
            var tread = _db.Treads.Single(t => t.Id == id);

            _db.Entry(tread).Collection(t => t.Posts).Load();
            foreach (var post in tread.Posts) _db.Entry(post).Reference(p => p.Image).Load();

            tread.Posts = tread.Posts.OrderBy(p => p.Time).ToList();

            return View(new TreadViewModel(tread));
        }
    }
}
