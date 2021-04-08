using Netaba.Data.Enums;

namespace Netaba.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public User() { }

        public User(string name, Role role, string password) : this(name, role)
        {
            Password = password;
        }

        public User(int id, string name, Role role) : this(name, role)
        {
            Id = id;
        }

        private User(string name, Role role)
        {
            Name = name;
            Role = role;
        }
    }
}
