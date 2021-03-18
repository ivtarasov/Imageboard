using System.ComponentModel.DataAnnotations;

namespace Netaba.Data.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Name is not specified.")]
        public string Name { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is not specified.")]
        public string Password { get; set; }
    }
}
