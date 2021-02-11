using System.ComponentModel.DataAnnotations;

namespace Imageboard.Data
{
    public enum Destination
    {
        [Display(Name = "Треду")]
        Tread,
        [Display(Name = "Доске")]
        Board
    }
}