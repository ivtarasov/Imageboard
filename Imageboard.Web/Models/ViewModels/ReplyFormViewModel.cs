using Imageboard.Data;
using Imageboard.Data.Enteties;

namespace Imageboard.Web.Models.ViewModels
{
    public class ReplyFormViewModel
    {
        private ReplyFormAction action;
        private string actionNameInView;
        public Tread ChageTargetTread { get; set; }
        public Board ChageTargetBoard { get; set; }
        public ReplyFormViewModel(ReplyFormAction act, Tread tread)
        {
            Action = act;
            ChageTargetTread = tread;
        }
        public ReplyFormViewModel(ReplyFormAction act, Board board)
        {
            Action = act;
            ChageTargetBoard = board;
        }
        public ReplyFormViewModel(ReplyFormAction act, Board board, Tread tread)
        {
            Action = act;
            ChageTargetBoard = board;
            ChageTargetTread = tread;
        }
        public string ActionNameInView
        {
            get => actionNameInView;
            private set => actionNameInView = value;
        }
        public ReplyFormAction Action
        {
            get => action;
            set
            {
                action = value;
                switch (action)
                {
                    case ReplyFormAction.CreateTread:
                        ActionNameInView = "Создать тред";
                        break;
                    case ReplyFormAction.ReplyInTread:
                        ActionNameInView = "Ответить в тред";
                        break;
                }
            }
        } 
    }
}
