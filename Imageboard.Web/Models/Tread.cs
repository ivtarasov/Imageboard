using System.Collections.Generic;

namespace Imageboard.Web.Models
{
    public class Tread
    {
        public int Id { get; set; }
        public List<Post> Posts { get; set; }
        public Board Board { get; set; }
        public int BoardId { get; set; }
        public Tread()
        {
            Posts = new List<Post>();
        }
    }
}
