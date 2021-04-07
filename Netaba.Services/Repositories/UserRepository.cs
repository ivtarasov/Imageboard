using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netaba.Data.Contexts;
using Netaba.Data.Models;
using Netaba.Data.Services.Hashing;
using Netaba.Services.Mappers;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Netaba.Services.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _context;
        private readonly ILogger _logger;
        public UserRepository(UsersDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> TryAddUserAsync(User user)
        {
            _context.Users.Add(user.ToEntity());

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes in the TryAddUserAsync method.");
                return false;
            }

            return true;
        }

        public async Task<bool> TryDeleteUserAsync(User user)
        {
            var userEntity = user.ToEntity();
            _context.Attach(userEntity);
            _context.Remove(userEntity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes in the TryDeleteUserAsync method.");
                return false;
            }

            return true;
        }

        public async Task<User> FindUserAsync(string name, string password)
        {
            var passHash = HashGenerator.GetHash(password);
            // TODO: 
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Name == name && Enumerable.SequenceEqual(u.PassHash, passHash));

            return user?.ToModel();
        }

        public async Task<User> FindUserAsync(string name)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Name == name);

            return user?.ToModel();
        }
    }
}
