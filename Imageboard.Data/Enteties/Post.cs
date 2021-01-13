using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Imageboard.Data.Enteties
{
    [Table("Post")]
    public class Post
    {
        [BindNever]
        public int Id { get; set; }
        public string PosterName { get; set; }
        public DateTime PostTime { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public int NumberInTread { get; set; }
        public Tread Tread { get; set; }
        public int TreadId { get; set; }
    }
}
