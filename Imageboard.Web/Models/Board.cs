using System.Collections.Generic;

namespace Imageboard.Web.Models
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
