using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Imageboard.Data.Enteties
{
    [Table("Post")]
    public class Post
    {
        [BindNever]
        public int Id { get; set; }
        public string PosterName { get; set; }
        [DisplayFormat(DataFormatString = "G")]
        public DateTime PostTime { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public int NumberInTread { get; set; }
        public bool IsSage { get; set; }
        public Tread Tread { get; set; }
        public int TreadId { get; set; }

        public Post() { }

        public Post(string message, string title, DateTime postTime, bool isSage, Tread tread, int numberInTread)
        {
            Message = message;
            Title = title;
            PostTime = postTime;
            IsSage = isSage;
            Tread = tread;
            NumberInTread = numberInTread;
        }

        public Post(string message, string title, DateTime postTime, bool isSage)
        {
            Message = message;
            Title = title;
            PostTime = postTime;
            IsSage = isSage;
        }
    }
}
