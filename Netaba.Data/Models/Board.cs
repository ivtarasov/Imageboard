using System.Collections.Generic;

namespace Netaba.Data.Models
{
    public class Board
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Tread> Treads { get; set; }

        public Board() { }

        public Board(string name, string descrition, List<Tread> treads)
        {
            Name = name;
            Description = descrition;
            Treads = treads;
        }

        public Board(string name, string descrition)
        {
            Name = name;
            Description = descrition;
        }
    }
}
