using Imageboard.Data;

namespace Imageboard.Web.Models.ViewModels
{
    public class ReplyFormViewModel
    {
        private ReplyFormAction action;
        private string actionNameInView;
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
        public string ActionNameInView
        {
            get => actionNameInView;
            private set => actionNameInView = value;
        }
        public int ChangeTargetId { get; set; } //Tread or Board id
        
    }
}
