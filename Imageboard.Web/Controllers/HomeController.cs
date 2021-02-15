using Imageboard.Data.Contexts;
using Imageboard.Data.Enteties;
using Imageboard.Data;
using Imageboard.Markup;
using Imageboard.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System;


namespace Imageboard.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly Random _random = new Random();
        
        public HomeController(ApplicationDbContext context)
        {
            _db = context;
            if (!(_db.Treads.Any() && _db.Boards.Any()))
            {
                Board board;

                if (_db.Boards.Any())
                {
                    board = _db.Boards.First();
                } 
                else
                {
                    board = new Board();
                }

                var treads = new List<Tread>();
                for (int i = 0; i < 10; i++) 
                {
                    var tread = new Tread(board);
                    var posts = new List<Post>();

                    for (int j = 0; j < 50; j++)
                    {
                        posts.Add(new Post(j == 0 ? $"Opening post. {j}" : RandomString(_random.Next(10, 500)),
                                           j == 0 ? $"Title of opening post.{j}" : RandomString(_random.Next(0, 5)),
                                           DateTime.Now, tread, j));
                    }

                    tread.Posts.AddRange(posts);
                    treads.Add(tread);
                }

                board.Treads.AddRange(treads);
                _db.Boards.Update(board);
                _db.SaveChanges();
            }
        }

        private string RandomString(int length)
        {
            var chars = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" +
                                 "абвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789" + "\n\n";
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[_random.Next(s.Length)])
                                        .ToArray());
        }

        [HttpPost]
        public IActionResult Delete(Dictionary<int, int> ids, int boardId)
        {
            var posts = _db.Posts.Where(p => ids.Values.Contains(p.Id));
            var openingPosts = posts.Where(p => p.NumberInTread == 0);

            DeleteTreads(openingPosts);
            DeletePosts(posts.Except(openingPosts));
            _db.SaveChanges();

            return RedirectToAction("DisplayBoard", new { id = boardId });
        }

        [NonAction]
        public void DeletePosts(IEnumerable<Post> posts)
        {
            _db.Posts.RemoveRange(posts);
        }

        [NonAction]
        public void DeleteTreads(IEnumerable<Post> openingPosts)
        {
            var treadIds = openingPosts.Select(p => p.TreadId);
            var treads = _db.Treads.Where(t => treadIds.Contains(t.Id));

            _db.Treads.RemoveRange(treads);
        }

        [HttpPost]
        public IActionResult ReplyToTread(string message, string title, int treadId, Destination dest)
        {
            var tread = _db.Treads.Single(t => t.Id == treadId);
            _db.Entry(tread).Collection(t => t.Posts).Load();

            tread.Posts.Add(new Post(Parser.ToHtml(message, _db), title, DateTime.Now, tread, tread.Posts.Count));

            _db.Update(tread);
            _db.SaveChanges();

            if (dest == Destination.Tread) return RedirectToAction("DisplayTread", new { id = treadId });
            else return RedirectToAction("DisplayBoard", new { id = tread.BoardId });
        }

        public IActionResult StartNewTread(string message, string title, int boardId, Destination dest)
        {
            var board = _db.Boards.Single(t => t.Id == boardId);
            _db.Entry(board).Collection(b => b.Treads).Load();

            var oPost = new Post(Parser.ToHtml(message, _db), title, DateTime.Now);
            var tread = new Tread(board, oPost);

            board.Treads.Add(tread);

            _db.Update(board);
            _db.SaveChanges();

            if (dest == Destination.Board) return RedirectToAction("DisplayBoard", new { id = boardId });
            else return RedirectToAction("DisplayTread", new { id = tread.Id });
        }

        [HttpGet]
        public IActionResult DisplayBoard(int id = 1)
        {
            var board = _db.Boards.Single(b => b.Id == id);

            _db.Entry(board).Collection(b => b.Treads).Load();

            foreach (var tread in board.Treads)
            {
                _db.Entry(tread).Collection(t => t.Posts).Load();
                tread.Posts = tread.Posts.OrderBy(p => p.NumberInTread).ToList();
            }

            return View(new BoardViewModel(board));
        }

        [HttpGet]
        public IActionResult DisplayTread(int id)
        {
            var tread = _db.Treads.Single(t => t.Id == id);

            _db.Entry(tread).Collection(t => t.Posts).Load();
            _db.Entry(tread).Reference(t => t.Board).Load();

            tread.Posts = tread.Posts.OrderBy(p => p.NumberInTread).ToList();
            return View(new TreadViewModel(tread));
        }
    }
}
