using Microsoft.Extensions.Options;
using Netaba.Data.Contexts;
using Netaba.Data.Enteties;
using Netaba.Data.Enums;
using Netaba.Data.Services.Hashing;
using System.Linq;
using System.Threading.Tasks;

namespace Netaba.Data.Services.Seeding
{
    public static class DataSeeder
    {
        public static async Task SeedUsersDbAsync(UsersDbContext context, IOptions<UsersDbSeedingConfiguration> options)
        {
            if (context.Users.Any()) return;

            var config = options.Value;
            User superAdmin = new()
            {
                Name = config.SuperAdminName,
                PassHash = HashGenerator.GetHash(config.SuperAdminPassword),
                Role = Role.SuperAdmin
            };

            context.Users.Add(superAdmin);
            await context.SaveChangesAsync();
        }

        public static async Task SeedBoardsDbAsync(BoardsDbContext context, IOptions<BoardsDbSeedingConfiguration> options)
        {
            if (context.Boards.Any()) return;

            var config = options.Value;

            context.Boards.AddRange(config.Boards);
            await context.SaveChangesAsync();
        }
    }
}
