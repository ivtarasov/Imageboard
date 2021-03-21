using Netaba.Data.Models;
using Netaba.Data.ViewModels;
using Netaba.Services.Pass;
using System.Collections.Generic;
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
        public static BoardEntety ToEntety(this Board board) =>
            new()
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                Treads = board.Treads?.Select(t => t.ToEntety())?.ToList()
            };

        public static TreadEntety ToEntety(this Tread tread) =>
            new()
            {
                Id = tread.Id,
                Posts = tread.Posts?.Select(p => p.ToEntety())?.ToList()
            };

        public static PostEntety ToEntety(this Post post) =>
            new()
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

        public static ImageEntety ToEntety(this Image image) =>
            new()
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

        public static UserEntety ToEntety(this User user) =>
            new()
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role,
                PassHash = HashGenerator.GetHash(user.Password ?? "")
            };

        public static CreatePostViewModel ToCreatePostViewModel(this Tread tread, string boardName, string boardDescription, Post post = null)
        {
            var treadViewModel = new TreadViewModel(tread.Posts.Select((p, i) =>
                                    new PostViewModel(p, ++i, false)).ToList(), tread.Id);

            return new CreatePostViewModel(new List<TreadViewModel> { treadViewModel }, boardName, post, boardDescription, tread.Id);
        }

        public static CreatePostViewModel ToCreatePostViewModel(this Board board, int postsFromTreadOnBoardView, int count, int pageSize, int page = 1, Post post = null)
        {
            var pageViewModel = new PageViewModel(count, page, pageSize, board.Name);

            var treadViewModels = board.Treads.Select(t =>
                                    new TreadViewModel(t.Posts.Select((p, i) =>
                                        new PostViewModel(p, ++i, true)).ToList(), postsFromTreadOnBoardView, t.Id)).ToList();

            return new CreatePostViewModel(treadViewModels, board.Name, post, board.Description, pageViewModel);
        }
    }
}
