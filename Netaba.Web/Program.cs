using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Netaba.Data.Contexts;
using Netaba.Data.Services.Seeding;
using System;
using System.Threading.Tasks;

namespace Netaba.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var usersDbContext = services.GetRequiredService<UsersDbContext>();
                var usersDbSeedingOptions = services.GetRequiredService<IOptions<UsersDbSeedingConfiguration>>();

                var boardsDbContext = services.GetRequiredService<BoardsDbContext>();
                var boardsDbSeedingOptions = services.GetRequiredService<IOptions<BoardsDbSeedingConfiguration>>();

                try
                {
                    await usersDbContext.Database.MigrateAsync();
                    await boardsDbContext.Database.MigrateAsync();

                    await DataSeeder.SeedUsersDbAsync(usersDbContext, usersDbSeedingOptions);
                    await DataSeeder.SeedBoardsDbAsync(boardsDbContext, boardsDbSeedingOptions);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the dbs.");
                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
