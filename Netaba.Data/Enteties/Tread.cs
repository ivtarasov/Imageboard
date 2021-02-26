using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Netaba.Data.Enteties
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

        public Tread(Post OPost) => Posts = new List<Post>{ OPost };
    }
}
