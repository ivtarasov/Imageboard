using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Imageboard.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Imageboard.Data.Contexts;
using Imageboard.Data.Enteties;
using Imageboard.Data;

namespace Imageboard.Web.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;
        private readonly Random random = new Random();

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            //_logger = logger;
            db = context;

            if (db.Boards.Count() == 0)
            {
                Board board = new Board();
                List<Tread> treads = new List<Tread>();
                for (int i = 0; i < 10; i++) 
                {
                    Tread tread = new Tread() { Board = board };
                    List<Post> posts = new List<Post>();
                    for (int j = 0; j < 50; j++)
                    {
                        posts.Add(new Post()
                        {
                            Message = j == 0 ? "Opening post." : RandomString(random.Next(10, 500)),
                            Title = j == 0 ? "Title of opening post." : RandomString(random.Next(0, 5)),
                            PostTime = DateTime.Now,
                            Tread = tread,
                            NumberInTread = j
                        });
                    }
                    tread.Posts.AddRange(posts);
                    treads.Add(tread);
                }
                board.Treads.AddRange(treads);
                db.Boards.Add(board);
                db.SaveChanges();
            }
        }
        public string RandomString(int length)
        {
            const string chars = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" +
                                 "абвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789" + "\n\n";
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());
        }
        /*[HttpPost]
        public IActionResult Delete(Dictionary<int, int> ids)
        {
            var replies = from r in db.Posts
                          where ids.Values.Contains(r.Id)
                            select r;
            foreach (var id in ids)
            {
                Debug.WriteLine($"{id} -- Полученное id.");
            }
            db.Posts.RemoveRange(replies);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/
        [HttpPost]
        public IActionResult ReplyInTread(Post post, int treadId)
        {
            var tread = db.Treads.Single(t => t.Id == treadId);
            post.Tread = tread;
            post.PostTime = DateTime.Now;
            db.Posts.Add(post);
            db.SaveChanges();

            return RedirectToAction("DisplayTread", new { id = treadId });
        }
        public IActionResult CreateTread(Post openingPost, int boardId)
        {
            var board = db.Boards.Single(t => t.Id == boardId);
            openingPost.PostTime = DateTime.Now;
            Tread tread = new Tread() { Board = board, Posts = { openingPost } };
            db.Treads.Add(tread);
            db.SaveChanges();
            return RedirectToAction("DisplayBoard", new { id = boardId });
        }
        [HttpGet]
        public IActionResult DisplayBoard(int id = 1)
        {
            var board = db.Boards.Single(b => b.Id == id);
            db.Entry(board).Collection(b => b.Treads).Load();
            foreach (var tread in board.Treads)
            {
                db.Entry(tread).Collection(t => t.Posts).Load();
                tread.Posts = tread.Posts.OrderBy(p => p.NumberInTread).ToList();
            }
            return View(new BoardViewModel() { Board = board });
        }
        [HttpGet]
        public IActionResult DisplayTread(int id)
        {
            var tread = db.Treads.Single(t => t.Id == id);
            db.Entry(tread).Collection(t => t.Posts).Load();
            tread.Posts = tread.Posts.OrderBy(p => p.NumberInTread).ToList();
            return View(new TreadViewModel()
            {
                Tread = tread,
                AboutOmittedPosts = "",
                IsShortcut = false
            });
        }

        /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}
