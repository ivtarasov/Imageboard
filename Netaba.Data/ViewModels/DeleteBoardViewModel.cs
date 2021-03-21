using System.ComponentModel.DataAnnotations;

namespace Netaba.Data.ViewModels
{
    public class DeleteBoardViewModel
    {
        public string BoardName { get; }

        public DeleteBoardViewModel(string boardName) => BoardName = boardName;
    }
}
