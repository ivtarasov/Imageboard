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
        public int? TreadId { get; }
        public Post Post { get;  }

        public CreatePostViewModel(List<TreadViewModel> treadViewModels, ReplyFormAction action, Post post, string boardName, int? treadId)
            : this(treadViewModels, action, boardName, treadId)
        {
            Post = post;
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