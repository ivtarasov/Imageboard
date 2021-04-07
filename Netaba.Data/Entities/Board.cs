using System.Collections.Generic;

namespace Netaba.Data.Entities
{
    public class Board
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Tread> Treads { get; set; }
    }
}
