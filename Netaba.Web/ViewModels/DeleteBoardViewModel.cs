using System.ComponentModel.DataAnnotations;

namespace Netaba.Web.ViewModels
{
    public class DeleteBoardViewModel
    {
        [Required(ErrorMessage = "Name is not specified.")]
        public string BoardName { get; }

        public DeleteBoardViewModel(string boardName) => BoardName = boardName;
    }
}
