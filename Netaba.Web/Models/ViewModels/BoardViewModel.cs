using Netaba.Data.Enteties;

namespace Netaba.Web.Models.ViewModels
{
    public class BoardViewModel
    {
        public Board Board { get; init; }

        public BoardViewModel(Board board) => Board = board;
    }
}
