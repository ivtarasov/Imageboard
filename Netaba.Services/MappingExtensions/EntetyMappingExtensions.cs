using ImageEntety = Netaba.Data.Enteties.Image;
using PostEntety = Netaba.Data.Enteties.Post;
using TreadEntety = Netaba.Data.Enteties.Tread;
using BoardEntety = Netaba.Data.Enteties.Board;
using System.Linq;
using Netaba.Data.Models;

namespace Netaba.Services.Mappers
{
    public static class EntetyMappingExtensions 
    {
        public static Board ToModel(this BoardEntety board) =>
            new(board.Name, board.Description, board.Treads.Select(t => t.ToModel()).ToList());

        public static Tread ToModel(this TreadEntety tread) =>
            new(tread.Id, tread.Posts.Select(p => p.ToModel()).ToList(), tread.BoardId);

        public static Post ToModel(this PostEntety post) =>
            new(post.Id, post.Message, post.Title, post.Time, post.Image?.ToModel(), post.IsOp, post.IsSage, post.TreadId, post.Tread?.Board?.Name);

        public static Image ToModel(this ImageEntety image) =>
            new(image.Id, image.Path, image.Name, image.Format, image.SizeDesc, image.Height, image.Width, image.ViewHeight, image.ViewWidth);
    }
}
