using System;

namespace Netaba.Data.Enteties
{
    public class Post
    {
        public int Id { get; set; }
        public string PosterName { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public bool IsOp { get; set; }
        public bool IsSage { get; set; }
        public byte[] PassHash { get; set; }
        public Image Image { get; set; }
        public int? PictureId { get; set; }
        public Tread Tread { get; set; }
        public int TreadId { get; set; }
        public int BoardId { get; set; }
    }
}
