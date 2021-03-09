using System.Collections.Generic;

namespace Netaba.Data.Enteties
{
    public class Tread
    {
        public int Id { get; set; }
        public List<Post> Posts { get; set; }
        public Board Board { get; set; }
        public int BoardId { get; set; }
    }
}