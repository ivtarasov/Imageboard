using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Netaba.Data.Contexts;
using Netaba.Services.ImageHandling;
using Netaba.Services.Markup;
using Netaba.Services.Repository;
using Netaba.Web.Infrastructure.Binders;
using Netaba.Web.Infrastructure.Filters;

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
            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("UsersConnection")));

            services.AddDbContext<BoardDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BoardsConnection")));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/login");
                        options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/login");
                    });

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(NavBarFilter));
                options.ModelBinderProviders.Insert(0, new PostBinderProvider());
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBoardRepository, BoardRepository>();
            services.AddScoped<IParser, Parser>();
            services.AddScoped<IImageHandler, ImageHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/exeption");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/code?code={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
