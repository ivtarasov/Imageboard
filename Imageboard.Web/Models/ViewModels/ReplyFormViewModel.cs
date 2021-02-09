using Imageboard.Data;

namespace Imageboard.Web.Models.ViewModels
{
    public class ReplyFormViewModel
    {
        private ReplyFormAction _action;
        public string ActionNameInView { get; private set; }
        public int TargetId { get; private set; }

        public ReplyFormViewModel(ReplyFormAction act, int targetId)
        {
            Action = act;
            TargetId = targetId;
        }

        public ReplyFormAction Action
        {
            get => _action;
            private set
            {
                _action = value;
                ActionNameInView = _action == ReplyFormAction.ReplyInTread ? "Ответить в тред" : "Создать тред";
              
            }
        } 
    }
}
