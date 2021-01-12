using System.Collections.Generic;

namespace Imageboard.Data.Enteties
{
    public class Board
    {
        public int Id { get; set; }
        public List<Tread> Treads { get; set; }
        public Board()
        {
            Treads = new List<Tread>();
        }
    }
}
