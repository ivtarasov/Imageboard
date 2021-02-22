using Imageboard.Data.Enums;

namespace Imageboard.Web.Models.ViewModels
{
    public class ReplyFormViewModel
    {
        private ReplyFormAction _action;
        public string ActionNameInView { get; init; }
        public int TargetId { get; init; }

        public ReplyFormViewModel(ReplyFormAction act, int targetId)
        {
            Action = act;
            TargetId = targetId;
        }

        public ReplyFormAction Action
        {
            get => _action;
            init
            {
                _action = value;
                ActionNameInView = _action == ReplyFormAction.ReplyToTread ? "Reply to the Tread " : "Start a New Tread";
            }
        } 
    }
}
