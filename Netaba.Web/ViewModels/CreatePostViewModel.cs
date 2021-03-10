using System.Collections.Generic;
using Netaba.Data.Models;
using Netaba.Data.Enums;

namespace Netaba.Web.ViewModels
{
    public class CreatePostViewModel
    {
        public List<TreadViewModel> TreadViewModels { get; }
        public ReplyFormAction Action { get; }
        public string BoardName { get; }
        public bool IsBoardPage { get; }
        public int? TreadId { get; }
        public Post Post { get;  }
        public PageViewModel PageViewModel { get; }

        public CreatePostViewModel(List<TreadViewModel> treadViewModels, ReplyFormAction action, Post post, string boardName, int? treadId)
            : this(treadViewModels, action, boardName, treadId)
        {
            Post = post;
        }

        public CreatePostViewModel(List<TreadViewModel> treadViewModels, ReplyFormAction action, Post post, string boardName, PageViewModel pageViewModel)
            : this(treadViewModels, action, boardName, pageViewModel)
        {
            Post = post;
        }

        public CreatePostViewModel(List<TreadViewModel> treadViewModels, ReplyFormAction action, string boardName, PageViewModel pageViewModel)
        {
            TreadViewModels = treadViewModels;
            Action = action;
            BoardName = boardName;
            IsBoardPage = true;
            PageViewModel = pageViewModel;
        }

        public CreatePostViewModel(List<TreadViewModel> treadViewModels, ReplyFormAction action, string boardName, int? treadId)
        {
            TreadViewModels = treadViewModels;
            Action = action;
            BoardName = boardName;
            TreadId = treadId;
        }
    }
}