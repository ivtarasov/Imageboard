using System.Collections.Generic;

namespace Netaba.Data.Models
{
    public class Tread
    {
        public int Id { get; }
        public List<Post> Posts { get; }
        public int? BoardId { get; }

        public Tread(int id, List<Post> posts, int boardId) : this(posts)
        {
            Id = id;
            BoardId = boardId;
        }

        public Tread(List<Post> posts) => Posts = posts;
    }
}
