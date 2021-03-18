using Netaba.Data.Enums;

namespace Netaba.Data.Models
{
    public class User
    {
        public string Name { get; }
        public string Password { get; }
        public Role? Role { get; }

        public User(string name, Role? role, string password)
        {
            Name = name;
            Password = password;
            Role = role;
        }

        public User(string name, Role? role)
        {
            Name = name;
            Role = role;
        }
    }
}
