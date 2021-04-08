using Netaba.Models;

namespace Netaba.Web.ViewModels
{
    public class AddBoardViewModel
    {
        public Board  Board { get; }

        public AddBoardViewModel(Board board) => Board = board;
    }
}
