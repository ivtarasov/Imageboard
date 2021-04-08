using Netaba.Models;
using System.Threading.Tasks;

namespace Netaba.Services.Repository
{
    public interface IUserRepository
    {
        public Task<bool> TryAddUserAsync(User user);
        public Task<bool> TryDeleteUserAsync(User user);

        public Task<User> FindUserAsync(string name, string password);
        public Task<User> FindUserAsync(string name);
    }
}