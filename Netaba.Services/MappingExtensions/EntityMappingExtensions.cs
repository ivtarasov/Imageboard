using Netaba.Models;
using System.Linq;
using BoardEntity = Netaba.Data.Entities.Board;
using ImageEntity = Netaba.Data.Entities.Image;
using PostEntity = Netaba.Data.Entities.Post;
using TreadEntity = Netaba.Data.Entities.Tread;
using UserEntity = Netaba.Data.Entities.User;

namespace Netaba.Services.Mappers
{
    public static class EntityMappingExtensions 
    {
        public static Board ToModel(this BoardEntity board)
        {
            return new(board.Id, board.Name, board.Description, board.Treads?.Select(t => t.ToModel())?.ToList());
        }

        public static Tread ToModel(this TreadEntity tread)
        {
            return new(tread.Id, tread.Posts?.Select(p => p.ToModel())?.ToList(), tread.BoardId);
        }

        public static Post ToModel(this PostEntity post)
        {
            return new(post.Id, post.PosterName, post.Message, post.Title, post.Time, post.Image?.ToModel(), post.IsOp, post.IsSage, post.TreadId, post.Tread?.Board?.Name);
        }

        public static Image ToModel(this ImageEntity image) 
        {
            return new(image.Id, image.Path, image.Name, image.Format, image.SizeDesc, image.Height, image.Width, image.ViewHeight, image.ViewWidth);
        }

        public static User ToModel(this UserEntity user) 
        {
            return new(user.Id, user.Name, user.Role);
        }
    }
}