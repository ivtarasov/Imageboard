using Netaba.Data.Models;
using Netaba.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;


namespace Netaba.Web.Services.Mappers
{
    public static class ModelMappingExtensions
    {
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
