using System.Collections.Generic;

namespace Netaba.Data.Models
{
    public class Board
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public List<Tread> Treads { get; private set; }

        public Board(int id, List<Tread> treads) : this(treads)
        {
            Id = id;
        }

        public Board(List<Tread> treads) => Treads = treads;
    }
}
