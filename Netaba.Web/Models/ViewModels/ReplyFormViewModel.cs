using Netaba.Data.Enums;
using Netaba.Data.Enteties;

namespace Netaba.Web.Models.ViewModels
{
    public class ReplyFormViewModel
    {
        private ReplyFormAction _action;
        public string ActionNameInView { get; private set; }
        public int TargetId { get; private set; }
        public Post Post { get; private set; }

        public ReplyFormViewModel(ReplyFormAction act, int targetId, Post post)
        {
            Action = act;
            TargetId = targetId;
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
