using Netaba.Data.Contexts;
using Netaba.Services.Markup;
using Netaba.Services.ImageHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Netaba.Services.Repository;
using Netaba.Web.Infrastructure.Binders;

namespace Netaba.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connection));

            services.AddControllersWithViews(options =>
            {
                options.ModelBinderProviders.Insert(0, new TimeBinderProvider());
                options.ModelBinderProviders.Insert(1, new PassHashBinderProvider());
            });

            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IParser, Parser>();
            services.AddSingleton<IImageHandler, ImageHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
