using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Imageboard.Data.Enteties
{
    [Table("Post")]
    public class Post
    {
        [BindNever]
        public int Id { get; set; }
        public string PosterName { get; set; }
        [DisplayFormat(DataFormatString = "G")]
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public bool IsOp { get; set; }
        public bool IsSage { get; set; }
        public byte[] IpAndPasswordHash { get; set; }
        public Image Image { get; set; }
        public int? PictureId { get; set; }
        public Tread Tread { get; set; }
        public int TreadId { get; set; }

        public Post() { }

        public Post(string message, string title, DateTime postTime, Image pic, bool isOp, bool isSage, string ip, string password)
        {
            Message = message;
            Title = title;
            Time = postTime;
            Image = pic;
            IsOp = isOp;
            IsSage = isSage;
            IpAndPasswordHash = MD5.HashData(Encoding.UTF8.GetBytes(ip + (password ?? "12345")));
        }
    }
}
