using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Netaba.Data.Models
{
    public class Post : IValidatableObject
    {
        public int Id { get; private set; }
        public string PosterName { get; private set; }
        [DisplayFormat(DataFormatString = "G")]
        public DateTime Time { get; private set; }
        public string Message { get; set; }
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new();

            if (string.IsNullOrEmpty(Message) && Image == null)
            {
                errors.Add(new ValidationResult("Message and Image cannot be empty at the same time."));
            }

            if ((Message?.Length ?? 0) > 1000)
            {
                errors.Add(new ValidationResult("Message length must be less than 1000 characters."));
            }

            if ((Title?.Length ?? 0) > 25)
            {
                errors.Add(new ValidationResult("Message length must be less than 1000 characters."));
            }

            return errors;
        }
    }
}
