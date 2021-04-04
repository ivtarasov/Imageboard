using System.Collections.Generic;

namespace Netaba.Data.Models
{
    public class Tread
    {
        public int Id { get; set; }
        public List<Post> Posts { get; set; }
        public int? BoardId { get; set; }

        public Tread() { }

        public Tread(int id, List<Post> posts, int boardId) : this(posts)
        {
            Id = id;
            BoardId = boardId;
        }

        public Tread(List<Post> posts)
        {
            Posts = posts;
        }
    }
}
