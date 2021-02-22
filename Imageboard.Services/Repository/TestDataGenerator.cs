using Imageboard.Data.Enteties;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Imageboard.Services.Repository
{
    public class TestDataGenerator
    {
        private static readonly Random _random = new Random(DateTime.Now.Second);
        private static readonly string _sourse = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789\n\n";

        public static Board GenerateData()
        {
            Board board = new Board();
            var treads = new List<Tread>();

            for (int i = 0; i < 10; i++)
            {
                var tread = new Tread(board);
                var posts = new List<Post>();

                for (int j = 0; j < 50; j++)
                    posts.Add(new Post(RandomString(_random.Next(10, 500)), RandomString(_random.Next(0, 5)),
                                       DateTime.Now, null, j == 0, false, tread));

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
