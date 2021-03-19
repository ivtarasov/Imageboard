using Netaba.Data.Models;
using System.Threading.Tasks;

namespace Netaba.Services.Repository
{
    public interface IUserRepository
    {
        public Task<bool> TryAddUser(User user);

        public Task<User> FindUserAsync(string name, string password);
        public Task<User> FindUseAsync(string name);
    }
}