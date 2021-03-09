using Netaba.Data.Enums;
using Netaba.Data.Models;

namespace Netaba.Web.ViewModels
{
    public class ReplyFormViewModel
    {
        public ReplyFormAction Action { get; }
        public string ActionNameInView { get; }
        public string BoardName { get; }
        public int? TreadId { get; }
        public Post Post { get; }

        public ReplyFormViewModel(ReplyFormAction act, string boardName, int? treadId, Post post)
        {
            Action = act;
            ActionNameInView = Action == ReplyFormAction.ReplyToTread ? "Reply to the Tread " : "Start a New Tread";
            BoardName = boardName;
            TreadId = treadId;
            Post = post;
        }
    }
}
