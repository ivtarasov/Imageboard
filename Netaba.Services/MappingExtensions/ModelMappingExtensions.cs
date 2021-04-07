using Netaba.Data.Models;
using Netaba.Data.Services.Hashing;
using System.Linq;
using BoardEntity = Netaba.Data.Entities.Board;
using ImageEntity = Netaba.Data.Entities.Image;
using PostEntity = Netaba.Data.Entities.Post;
using TreadEntity = Netaba.Data.Entities.Tread;
using UserEntity = Netaba.Data.Entities.User;


namespace Netaba.Services.Mappers
{
    public static class ModelMappingExtensions
    {
        public static BoardEntity ToEntity(this Board board)
        {
            return new()
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                Treads = board.Treads?.Select(t => t.ToEntity())?.ToList()
            };
        }

        public static TreadEntity ToEntity(this Tread tread)
        {
            return new()
            {
                Id = tread.Id,
                Posts = tread.Posts?.Select(p => p.ToEntity())?.ToList()
            };
        }

        public static PostEntity ToEntity(this Post post) 
        {
            return new()
            {
                Id = post.Id,
                PosterName = post.PosterName,
                Message = post.Message,
                Title = post.Title,
                Time = post.Time,
                IsOp = post.IsOp,
                IsSage = post.IsSage,
                Image = post.Image?.ToEntity(),
                PassHash = HashGenerator.GetHash(post.Ip, post.Password)
            };
        }

        public static ImageEntity ToEntity(this Image image)
        {
            return new()
            {
                Id = image.Id,
                Name = image.Name,
                SizeDesc = image.SizeDesc,
                Format = image.Format,
                Path = image.Path,
                Height = image.Height,
                Width = image.Width,
                ViewHeight = image.ViewHeight,
                ViewWidth = image.ViewWidth
            };
        }

        public static UserEntity ToEntity(this User user)
        {
            return new()
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role,
                PassHash = HashGenerator.GetHash(user.Password ?? "")
            };
        }
    }
}
