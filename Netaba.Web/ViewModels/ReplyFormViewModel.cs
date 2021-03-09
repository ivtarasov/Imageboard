using Netaba.Data.Enums;
using Netaba.Data.Models;

namespace Netaba.Web.ViewModels
{
    public class ReplyFormViewModel
    {
        private ReplyFormAction _action;
        public string ActionNameInView { get; private set; }
        public string BoardName { get; private set; }
        public int? TreadId { get; private set; }
        public Post Post { get; private set; }

        public ReplyFormViewModel(ReplyFormAction act, string boardName, int? treadId, Post post)
        {
            Action = act;
            BoardName = boardName;
            TreadId = treadId;
            Post = post;
        }

        public ReplyFormAction Action
        {
            get => _action;
            private set
            {
                _action = value;
                ActionNameInView = _action == ReplyFormAction.ReplyToTread ? "Reply to the Tread " : "Start a New Tread";
            }
        } 
    }
}
