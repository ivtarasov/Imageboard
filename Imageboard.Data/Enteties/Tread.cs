using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Imageboard.Data.Enteties
{
    [Table("Tread")]
    public class Tread
    {
        [BindNever]
        public int Id { get; set; }
        public List<Post> Posts { get; set; }
        public Board Board { get; set; }
        public int BoardId { get; set; }

        public Tread()
        {
            Posts = new List<Post>();
        }

        public Tread(List<Post> posts)
        {
            Posts = posts;
        }

        public Tread(Board board)
        {
            Board = board;
            Posts = new List<Post>();
        }

        public Tread(Board board, List<Post> posts)
        {
            Board = board;
            Posts = posts;
        }

        public Tread(Board board, Post openingPost)
        {
            Board = board;
            Posts = new List<Post>();
            openingPost.Tread = this;
            openingPost.NumberInTread = 0;
            Posts.Add(openingPost);
        }
    }
}
