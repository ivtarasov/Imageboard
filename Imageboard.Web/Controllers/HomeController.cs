using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Imageboard.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Imageboard.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BoardsContext db;
        private readonly Random random = new Random();

        public HomeController(ILogger<HomeController> logger, BoardsContext context)
        {
            _logger = logger;
            db = context;

            if (db.Boards.Count() == 0)
            {
                Board board = new Board();
                List<Tread> treads = new List<Tread>();
                for (int i = 0; i < 10; i++) 
                {
                    Tread tread = new Tread();
                    List<Post> posts = new List<Post>();
                    for (int j = 0; j < 50; j++)
                    {
                        posts.Add(new Post()
                        {
                            Message = RandomString(random.Next(10, 500)),
                            Title = RandomString(random.Next(0, 5)),
                            PostTime = DateTime.Now,
                            Tread = tread
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
        }
        [HttpPost]
        public IActionResult Reply(Post post)
        {
            Debug.WriteLine(post.Message ?? "Пусто!");
            post.PostTime = DateTime.Now;
            db.Posts.Add(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Index()
        {
            Tread tread = new Tread() { Posts = db.Posts.AsNoTracking().ToList() };
            return View(tread);
        }*/
        [HttpGet]
        public IActionResult Board()
        {
            var boards = db.Boards.Include(b => b.Treads)
                                  .ThenInclude(t => t.Posts).AsNoTracking();
            return View(boards.ToList().First());
        }
        [HttpGet]
        public IActionResult Tread(int id)
        {
            var boards = db.Boards.Include(b => b.Treads)
                                  .ThenInclude(t => t.Posts).AsNoTracking();
            var tread = boards.First().Treads.Find(t => t.Id == id);
            Debug.WriteLine($"{(tread.Id.ToString() ?? "null")}");
            TreadModel treadModel = new TreadModel()
            {
                Tread = tread,
                AboutOmittedPosts = "",
                FirstDisplayedPost = 1 + 1,
                IsShortcut = false
            };
            return View(treadModel);
        }

        /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}
