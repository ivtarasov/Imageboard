using Netaba.Data.Enteties;

namespace Netaba.Web.Models.ViewModels
{
    public class BoardViewModel
    {
        public Board Board { get; private set; }

        public BoardViewModel(Board board) => Board = board;
    }
}