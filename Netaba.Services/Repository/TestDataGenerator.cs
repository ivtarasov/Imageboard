using Netaba.Data.Enteties;
using Netaba.Services.Pass;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Netaba.Services.Repository
{
    public class TestDataGenerator
    {
        private static readonly Random _random = new Random(DateTime.Now.Second);
        private static readonly string _sourse = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789\n\n";

        public static Board GenerateData()
        {
            var board = new Board() { Treads = new List<Tread>() };
            var treads = new List<Tread>();

            for (int i = 0; i < 10; i++)
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

            board.Treads.AddRange(treads);
            return board;
        }

        private static string RandomString(int length) => 
            new string(Enumerable.Repeat(_sourse, length).Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}
