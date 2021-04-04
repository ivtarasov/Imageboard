using Netaba.Web.Enums;
using Netaba.Data.Models;

namespace Netaba.Web.ViewModels
{
    public class ReplyFormViewModel
    {
        public ReplyFormAction Action { get; }
        public string ActionNameInView { get; }
        public string BoardName { get; }
        public Post Post { get; }
        public int? TreadId { get; }

        public ReplyFormViewModel(ReplyFormAction action, string boardName, Post post, int? treadId)
        {
            Action = action;
            ActionNameInView = Action == ReplyFormAction.ReplyToTread ? "Reply to the Tread " : "Start a New Tread";
            BoardName = boardName;
            Post = post;
            TreadId = treadId;
        }
    }
}
