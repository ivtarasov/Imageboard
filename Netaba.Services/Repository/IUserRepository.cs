using Netaba.Data.Models;
using System.Threading.Tasks;

namespace Netaba.Services.Repository
{
    public interface IUserRepository
    {
        public Task<bool> TryAddUser(User user);

        public Task<User> FindUser(string name, string password);
        public Task<User> FindUser(string name);
    }
}