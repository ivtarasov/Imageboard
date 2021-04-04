using Netaba.Data.Models;
using Netaba.Data.Services.Hashing;
using System.Linq;
using BoardEntety = Netaba.Data.Enteties.Board;
using ImageEntety = Netaba.Data.Enteties.Image;
using PostEntety = Netaba.Data.Enteties.Post;
using TreadEntety = Netaba.Data.Enteties.Tread;
using UserEntety = Netaba.Data.Enteties.User;


namespace Netaba.Services.Mappers
{
    public static class ModelMappingExtensions
    {
        public static BoardEntety ToEntety(this Board board)
        {
            return new()
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                Treads = board.Treads?.Select(t => t.ToEntety())?.ToList()
            };
        }

        public static TreadEntety ToEntety(this Tread tread)
        {
            return new()
            {
                Id = tread.Id,
                Posts = tread.Posts?.Select(p => p.ToEntety())?.ToList()
            };
        }

        public static PostEntety ToEntety(this Post post) 
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
                Image = post.Image?.ToEntety(),
                PassHash = HashGenerator.GetHash(post.Ip, post.Password)
            };
        }

        public static ImageEntety ToEntety(this Image image)
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

        public static UserEntety ToEntety(this User user)
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
