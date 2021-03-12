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
        public string BoardDescription { get; }
        public bool IsBoardPage { get; }
        public int? TreadId { get; }

        public PageViewModel PageViewModel { get; }

        public CreatePostViewModel(List<TreadViewModel> treadViewModels, string boardName, string boardDescription, PageViewModel pageViewModel)
            : this(treadViewModels, boardName, boardDescription)
        {
            Action = ReplyFormAction.StartNewTread;
            IsBoardPage = true;
            PageViewModel = pageViewModel;
        }

        public CreatePostViewModel(List<TreadViewModel> treadViewModels, string boardName, string boardDescription, int treadId)
            : this(treadViewModels, boardName, boardDescription)
        {
            Action = ReplyFormAction.ReplyToTread;
            TreadId = treadId;
        }

        private CreatePostViewModel(List<TreadViewModel> treadViewModels, string boardName, string boardDescription)
        {
            TreadViewModels = treadViewModels;
            BoardName = boardName;
            BoardDescription = boardDescription;
        }
    }
}