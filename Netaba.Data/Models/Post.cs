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
        public string Message { get; set; }
        public string Title { get; }
        public bool IsOp { get; }
        public bool IsSage { get; }
        public string Password { get; }
        public string Ip { get; }
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

            if ((Message?.Length ?? 0) > 15_000)
            {
                errors.Add(new ValidationResult("Too long Message. Limit: 15000 characters."));
            }

            if ((Title?.Length ?? 0) > 35)
            {
                errors.Add(new ValidationResult("Too long Title. Limit: 35 characters."));
            }

            return errors;
        }
    }
}
