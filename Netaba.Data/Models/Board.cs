using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Netaba.Data.Models
{
    public class Board
    {
        [Required(ErrorMessage = "Name is not specified.")]
        [StringLength(5, ErrorMessage = "Too long name. Limit: 5 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is not specified.")]
        [StringLength(15, ErrorMessage = "Too long description. Limit: 15 characters.")]
        public string Description { get; set; }
        public List<Tread> Treads { get; set; }

        public Board() { }

        public Board(string name, string descrition, List<Tread> treads)
        {
            Name = name;
            Description = descrition;
            Treads = treads;
        }

        public Board(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
