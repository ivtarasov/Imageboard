using System;

namespace Imageboard.Web.Models
{
    public class Post
    {
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
