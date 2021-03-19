using Microsoft.EntityFrameworkCore;
using Netaba.Data.Contexts;
using Netaba.Data.Models;
using Netaba.Services.Mappers;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserEntety = Netaba.Data.Enteties.User;

namespace Netaba.Services.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        public UserRepository(UserDbContext context) => _context = context;

        public async Task<bool> TryAddUser(User user)
        {
            await _context.Users.AddAsync(user.ToEntety());

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<User> FindUserAsync(string name, string password)
        {
            // TODO: Write custom function mapping to use the Netaba.Services.Pass.PassChecker class here
            var passHash = MD5.HashData(Encoding.UTF8.GetBytes(password));
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name && Enumerable.SequenceEqual(u.PassHash, passHash));

            if (user == null) return null;
            return user.ToModel();
        }

        public async Task<User> FindUseAsync(string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);

            if (user == null) return null;
            return user.ToModel();
        }
    }
}
