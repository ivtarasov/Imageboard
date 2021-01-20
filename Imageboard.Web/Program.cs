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
            var sourse = "*te*sdf_sfr*sqweeeee_eeeeet*";
            Console.WriteLine(WakabaMark.MakeMarkUp(sourse));
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}