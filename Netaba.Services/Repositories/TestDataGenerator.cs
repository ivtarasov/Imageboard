using Netaba.Data.Enteties;
using Netaba.Services.Pass;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Netaba.Services.Repository
{
    public class TestDataGenerator
    {
        private static readonly Random _random = new(DateTime.Now.Second);
        private static readonly string _sourse = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789\n\n";

        public static List<Board> GenerateData()
        {
            (string Name, string Description)[] sboards = {("b", "bbb"), ("a", "aaa"), ("c", "ccc")};

            var boards = new List<Board>();
            foreach (var (Name, Description) in sboards)
            {
                var treads = new List<Tread>();
                var board = new Board { Name = Name, Description = Description };
                for (int i = 0; i < 50; i++)
                {
                    var tread = new Tread { Posts = new List<Post>() };
                    var posts = new List<Post>();

                    for (int j = 0; j < 50; j++)
                        posts.Add(new Post
                        {
                            Message = RandomString(_random.Next(1, 50)),
                            Title = RandomString(_random.Next(1, 5)),
                            Time = DateTime.Now,
                            IsOp = j == 0,
                            IsSage = false,
                            Tread = tread,
                            Image = null,
                            PassHash = HashGenerator.GetHash("127.0.0.1", "12345")
                        });

                    tread.Posts.AddRange(posts);
                    treads.Add(tread);
                }
                board.Treads = treads;
                boards.Add(board);
            }
            return boards;
        }

        private static string RandomString(int length) => 
            new(Enumerable.Repeat(_sourse, length).Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}
