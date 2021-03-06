using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Netaba.Data.Models
{
    public class Post
    {
        [BindNever]
        public int Id { get; private set; }
        public string PosterName { get; private set; }
        [DisplayFormat(DataFormatString = "G")]
        public DateTime Time { get; private set; }
        [Required]
        public string Message { get; private set; }
        [Required]
        public string Title { get; private set; }
        public bool IsOp { get; private set; }
        public bool IsSage { get; private set; }
        public byte[] PassHash { get; private set; }
        public Image Image { get; private set; }
        public int? TreadId { get; private set; }
        public int? BoardId { get; private set; }

        public Post(int id, string message, string title, DateTime postTime, Image image, bool isOp, bool isSage, byte[] hash, int treadId, int boardId)
            : this(message, title, postTime, image, isOp, isSage, hash, treadId, boardId)
        {
            Id = id;
        }

        public Post(string message, string title, DateTime postTime, Image image, bool isOp, bool isSage, byte[] hash, int treadId, int boardId)
            : this(message, title, postTime, image, isOp, isSage, hash)
        {
            TreadId = treadId;
            BoardId = boardId;
        }

        public Post(string message, string title, DateTime postTime, Image image, bool isOp, bool isSage, byte[] hash)
        {
            Message = message;
            Title = title;
            Time = postTime;
            Image = image;
            IsOp = isOp;
            IsSage = isSage;
            PassHash = hash;
        }
    }
}
