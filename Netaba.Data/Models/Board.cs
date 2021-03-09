using System.Collections.Generic;

namespace Netaba.Data.Models
{
    public class Board
    {
        public string Name { get; }
        public string Description { get; }
        public List<Tread> Treads { get; }

        public Board(string name, string descrition, List<Tread> treads)
        {
            Name = name;
            Description = descrition;
            Treads = treads;
        }
    }
}
