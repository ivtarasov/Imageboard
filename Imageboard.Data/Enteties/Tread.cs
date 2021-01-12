using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imageboard.Data.Enteties
{
    [Table("Tread")]
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
