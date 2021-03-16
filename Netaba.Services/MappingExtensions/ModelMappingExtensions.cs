using ImageEntety = Netaba.Data.Enteties.Image;
using PostEntety = Netaba.Data.Enteties.Post;
using TreadEntety = Netaba.Data.Enteties.Tread;
using BoardEntety = Netaba.Data.Enteties.Board;
using Netaba.Data.Models;
using Netaba.Data.ViewModels;
using Netaba.Services.Pass;
using System.Linq;
using System.Collections.Generic;


namespace Netaba.Services.Mappers
{
    public static class ModelMappingExtensions
    {
        public static BoardEntety ToEntety(this Board board) =>
            new()
            {
                Name = board.Name,
                Description = board.Description,
                Treads = board.Treads.Select(t => t.ToEntety()).ToList()
            };

        public static TreadEntety ToEntety(this Tread tread) =>
            new()
            {
                Posts = tread.Posts.Select(p => p.ToEntety()).ToList()
            };

        public static PostEntety ToEntety(this Post post) =>
            new()
            {
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
                Name = image.Name,
                SizeDesc = image.SizeDesc,
                Format = image.Format,
                Path = image.Path,
                Height = image.Height,
                Width = image.Width,
                ViewHeight = image.ViewHeight,
                ViewWidth = image.ViewWidth
            };

        public static CreatePostViewModel MapToCreatePostViewModel(this Tread tread, string boardName, string boardDescription, Post post = null)
        {
            var treadViewModel = new TreadViewModel(tread.Posts.Select((p, i) =>
                                    new PostViewModel(p, ++i, false)).ToList(), tread.Id);

            return new CreatePostViewModel(new List<TreadViewModel> { treadViewModel }, boardName, post, boardDescription, tread.Id);
        }

        public static CreatePostViewModel MapToCreatePostViewModel(this Board board, int postsFromTreadOnBoardView, int pageSize, int page = 1, Post post = null)
        {
            var count = board.Treads.Count;
            var pageViewModel = new PageViewModel(count, page, pageSize, board.Name);

            var treads = board.Treads.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var treadViewModels = treads.Select(t =>
                                    new TreadViewModel(t.Posts.Select((p, i) =>
                                        new PostViewModel(p, ++i, true)).ToList(), postsFromTreadOnBoardView, t.Id)).ToList();

            return new CreatePostViewModel(treadViewModels, board.Name, post, board.Description, pageViewModel);
        }
    }
}
