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
        public static async Task SeedAsync(UserDbContext context, IOptions<SeedingConfiguration> options)
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
    }
}
