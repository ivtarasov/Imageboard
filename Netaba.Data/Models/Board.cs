using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Netaba.Data.Models
{
    public class Board
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is not specified.")]
        [StringLength(5, ErrorMessage = "Too long name. Limit: 5 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is not specified.")]
        [StringLength(20, ErrorMessage = "Too long description. Limit: 20 characters.")]
        public string Description { get; set; }
        public List<Tread> Treads { get; set; }

        public Board() { }

        public Board(int id, string name, string description, List<Tread> treads) : this(name, description, treads)
        {
            Id = id;
        }

        public Board(string name, string description, List<Tread> treads)
        {
            Name = name;
            Description = description;
            Treads = treads;
        }
    }
}
