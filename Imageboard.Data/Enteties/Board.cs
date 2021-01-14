using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Imageboard.Data.Enteties
{
    public class Board
    {
        [BindNever]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Tread> Treads { get; set; }
        public Board()
        {
            Treads = new List<Tread>();
        }
        public Board(List<Tread> treads)
        {
            Treads = treads;
        }
    }
}
