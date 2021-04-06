using Microsoft.EntityFrameworkCore;
using Netaba.Data.Contexts;
using Netaba.Data.Models;
using Netaba.Data.Services.Hashing;
using Netaba.Services.Mappers;
using System.Linq;
using System.Threading.Tasks;

namespace Netaba.Services.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<bool> TryAddUserAsync(User user)
        {
            _context.Users.Add(user.ToEntety());

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

        public async Task<bool> TryDeleteUserAsync(User user)
        {
            var userEntety = user.ToEntety();
            _context.Attach(userEntety);
            _context.Remove(userEntety);

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
            var passHash = HashGenerator.GetHash(password);
            // TODO: 
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Name == name && Enumerable.SequenceEqual(u.PassHash, passHash));

            if (user == null) return null;
            return user.ToModel();
        }

        public async Task<User> FindUserAsync(string name)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Name == name);

            if (user == null) return null;
            return user.ToModel();
        }
    }
}
