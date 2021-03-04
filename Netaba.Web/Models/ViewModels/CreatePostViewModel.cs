using System.Collections.Generic;
using Netaba.Data.Enteties;
using Netaba.Data.Enums;

namespace Netaba.Web.Models.ViewModels
{
    public class CreatePostViewModel
    {
        public List<TreadViewModel> TreadViewModels { get; private set; }
        public ReplyFormAction Action { get; private set; }
        public int TargetId { get; private set; }
        public int BoardId { get; private set; }
        public Post Post { get; private set; }

        public CreatePostViewModel(List<TreadViewModel> treadViewModels, ReplyFormAction action, int targetId, int boardId)
        {
            TreadViewModels = treadViewModels;
            Action = action;
            TargetId = targetId;
            BoardId = boardId;
        }

        public CreatePostViewModel(List<TreadViewModel> treadViewModels, ReplyFormAction action, Post post, int targetId, int boardId) 
        {
            TreadViewModels = treadViewModels;
            Action = action;
            Post = post;
            TargetId = targetId;
            BoardId = boardId;
        }
    }
}