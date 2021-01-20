using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using Imageboard.Markup;

namespace Imageboard.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sourse = "*te**st*";
            Console.WriteLine(WakabaMark.MakeMarkUp(sourse));
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
