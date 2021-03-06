using System.Collections.Generic;

namespace Netaba.Data.Enteties
{
    public class Board
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Tread> Treads { get; set; }
    }
}
