using Netaba.Data.Enums;

namespace Netaba.Data.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public User(string name, Role role, string password)
        {
            Name = name;
            Password = password;
            Role = role;
        }

        public User(string name, Role role)
        {
            Name = name;
            Role = role;
        }
    }
}
