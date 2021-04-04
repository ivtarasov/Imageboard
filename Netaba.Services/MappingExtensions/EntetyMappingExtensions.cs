using Netaba.Data.Models;
using System.Linq;
using BoardEntety = Netaba.Data.Enteties.Board;
using ImageEntety = Netaba.Data.Enteties.Image;
using PostEntety = Netaba.Data.Enteties.Post;
using TreadEntety = Netaba.Data.Enteties.Tread;
using UserEntety = Netaba.Data.Enteties.User;

namespace Netaba.Services.Mappers
{
    public static class EntetyMappingExtensions 
    {
        public static Board ToModel(this BoardEntety board)
        {
            return new(board.Id, board.Name, board.Description, board.Treads?.Select(t => t.ToModel())?.ToList());
        }

        public static Tread ToModel(this TreadEntety tread)
        {
            return new(tread.Id, tread.Posts?.Select(p => p.ToModel())?.ToList(), tread.BoardId);
        }

        public static Post ToModel(this PostEntety post)
        {
            return new(post.Id, post.PosterName, post.Message, post.Title, post.Time, post.Image?.ToModel(), post.IsOp, post.IsSage, post.TreadId, post.Tread?.Board?.Name);
        }

        public static Image ToModel(this ImageEntety image) 
        {
            return new(image.Id, image.Path, image.Name, image.Format, image.SizeDesc, image.Height, image.Width, image.ViewHeight, image.ViewWidth);
        }

        public static User ToModel(this UserEntety user) 
        {
            return new(user.Id, user.Name, user.Role);
        }
    }
}