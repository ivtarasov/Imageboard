using ImageEntety = Netaba.Data.Enteties.Image;
using PostEntety = Netaba.Data.Enteties.Post;
using TreadEntety = Netaba.Data.Enteties.Tread;
using BoardEntety = Netaba.Data.Enteties.Board;
using Netaba.Data.Models;
using System.Linq;

namespace Netaba.Services.Mappers
{
    static class ModelMapper
    {
        public static BoardEntety ToEntety(Board board) =>
            new()
            {
                Treads = board.Treads.Select(t => ToEntety(t)).ToList()
            };

        public static TreadEntety ToEntety(Tread tread) =>
            new()
            {
                Posts = tread.Posts.Select(p => ToEntety(p)).ToList()
            };

        public static PostEntety ToEntety(Post post) =>
            new()
            {
                Message = post.Message,
                Title = post.Title,
                Time = post.Time,
                IsOp = post.IsOp,
                IsSage = post.IsSage,
                Image = ToEntety(post.Image),
                PassHash = post.PassHash
            };

        public static ImageEntety ToEntety(Image image)
        {
            if (image == null) return null;

            return new()
            {
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
    }
}
