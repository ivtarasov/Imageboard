using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Netaba.Data.Models
{
    public class Post : IValidatableObject
    {
        public int Id { get; }
        public string PosterName { get; }
        [DisplayFormat(DataFormatString = "G")]
        public DateTime Time { get; }
        [DataType(DataType.MultilineText)]
        [StringLength(15_000, ErrorMessage = "Too long message. Limit: 15000 characters.")]
        public string Message { get; set; }
        [StringLength(35, ErrorMessage = "Too long title. Limit: 35 characters.")]
        public string Title { get; }
        public bool IsOp { get; }
        public bool IsSage { get; }
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Too long password. Limit: 20 characters.")]
        public string Password { get; }
        public string Ip { get; }
        [DataType(DataType.Upload)]
        public Image Image { get; }
        public string BoardName { get; }
        public int? TreadId { get; }
        
        public Post(int id, string message, string title, DateTime postTime, Image image, bool isOp, bool isSage, int treadId, string boardName)
            : this(message, title, postTime, image, isOp, isSage)
        {
            Id = id;
            TreadId = treadId;
            BoardName = boardName;
        }

        public Post(string message, string title, DateTime postTime, string ip, string password, Image image, bool isOp, bool isSage)
            : this(message, title, postTime, image, isOp, isSage)
        {
            Ip = ip;
            Password = password;
        }

        private Post(string message, string title, DateTime postTime, Image image, bool isOp, bool isSage)
        {
            Message = message;
            Title = title;
            Time = postTime;
            Image = image;
            IsOp = isOp;
            IsSage = isSage;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new();

            if (string.IsNullOrEmpty(Message) && Image == null)
            {
                errors.Add(new ValidationResult("Post must contain Message or Image."));
            }

            return errors;
        }
    }
}
