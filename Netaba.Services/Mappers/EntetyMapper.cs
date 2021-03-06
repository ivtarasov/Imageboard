using ImageEntety = Netaba.Data.Enteties.Image;
using PostEntety = Netaba.Data.Enteties.Post;
using TreadEntety = Netaba.Data.Enteties.Tread;
using BoardEntety = Netaba.Data.Enteties.Board;
using System.Linq;
using Netaba.Data.Models;

namespace Netaba.Services.Mappers
{
    static class EntetyMapper
    {
        public static Board ToModel(BoardEntety board) =>
            new(board.Id, board.Treads.Select(t => ToModel(t)).ToList());

        public static Tread ToModel(TreadEntety tread) =>
            new(tread.Id, tread.Posts.Select(p => ToModel(p)).ToList(), tread.BoardId);

        public static Post ToModel(PostEntety post) =>
            new(post.Id, post.Message, post.Title, post.Time, post.Image != null ? ToModel(post.Image) : null, post.IsOp, post.IsSage, post.PassHash, post.TreadId, post.Tread.BoardId);

        public static Image ToModel(ImageEntety image) =>
            new(image.Id, image.Path, image.Name, image.Format, image.SizeDesc, image.Height, image.Width, image.ViewHeight, image.ViewWidth);
    }
}
