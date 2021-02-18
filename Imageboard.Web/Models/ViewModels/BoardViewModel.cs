using Imageboard.Data.Enteties;

namespace Imageboard.Web.Models.ViewModels
{
    public class BoardViewModel
    {
        public Board Board { get; private set; }

        public BoardViewModel(Board board) => Board = board;
    }
}
