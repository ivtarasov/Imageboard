using System.ComponentModel.DataAnnotations;

namespace Netaba.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Name is not specified.")]
        [StringLength(20, ErrorMessage = "Too long name. Limit: 20 characters.")]
        public string Name { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is not specified.")]
        [StringLength(20, ErrorMessage = "Too long password. Limit: 20 characters.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords are not equal.")]
        public string ConfirmPassword { get; set; }
    }
}
