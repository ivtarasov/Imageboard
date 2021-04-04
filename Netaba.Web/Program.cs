using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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
                var context = services.GetRequiredService<UserDbContext>();
                var seedingOptions = services.GetRequiredService<IOptions<SeedingConfiguration>>();
                try
                {
                    await context.Database.MigrateAsync();
                    await DataSeeder.SeedAsync(context, seedingOptions);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
