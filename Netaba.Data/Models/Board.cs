using System.Collections.Generic;

namespace Netaba.Data.Models
{
    public class Board
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<Tread> Treads { get; private set; }

        public Board(string name, string descrition, List<Tread> treads)
        {
            Name = name;
            Description = descrition;
            Treads = treads;
        }
    }
}
