using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Netaba.Data.Models
{
    public class Tread
    {
        [BindNever]
        public int Id { get; private set; }
        public List<Post> Posts { get; private set; }
        public int? BoardId { get; private set; }

        public Tread(int id, List<Post> posts, int boardId) : this(posts, boardId) => Id = id;

        public Tread(List<Post> posts, int boardId) : this(posts) => BoardId = boardId;

        public Tread(List<Post> posts) => Posts = posts;
    }
}
