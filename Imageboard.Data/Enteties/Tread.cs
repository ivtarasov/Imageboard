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

        public Tread() => Posts = new List<Post>();

        public Tread(Board board)
        {
            Board = board;
            Posts = new List<Post>();
        }

        public Tread(Board board, Post OPost)
        {
            Board = board;
            OPost.Tread = this;
            OPost.NumberInTread = 0;
            Posts = new List<Post>{ OPost };
        }
    }
}
