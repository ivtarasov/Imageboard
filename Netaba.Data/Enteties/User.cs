using Netaba.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Netaba.Data.Enteties
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] PassHash { get; set; }
        public Role? Role { get; set; }
    }
}
