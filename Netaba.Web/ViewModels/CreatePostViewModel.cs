using System.Collections.Generic;
using Netaba.Data.Models;
using Netaba.Data.Enums;

namespace Netaba.Web.ViewModels
{
    public class CreatePostViewModel
    {
        public List<TreadViewModel> TreadViewModels { get; private set; }
        public ReplyFormAction Action { get; private set; }
        public int TargetId { get; private set; }
        public int BoardId { get; private set; }
        public int? TreadId { get; private set; }
        public Post Post { get; private set; }

        public CreatePostViewModel(List<TreadViewModel> treadViewModels, ReplyFormAction action, Post post, int boardId, int treadId)
            : this(treadViewModels, action, boardId, treadId)
        {
            Post = post;
        }

        public CreatePostViewModel(List<TreadViewModel> treadViewModels, ReplyFormAction action, Post post, int boardId)
            : this(treadViewModels, action, boardId)
        {
            Post = post;
        }

        public CreatePostViewModel(List<TreadViewModel> treadViewModels, ReplyFormAction action, int boardId, int treadId)
        {
            TreadViewModels = treadViewModels;
            Action = action;
            BoardId = boardId;
            TreadId = treadId;
            TargetId = treadId;
        }

        public CreatePostViewModel(List<TreadViewModel> treadViewModels, ReplyFormAction action, int boardId)
        {
            TreadViewModels = treadViewModels;
            Action = action;
            BoardId = boardId;
            TargetId = boardId;
        }
    }
}