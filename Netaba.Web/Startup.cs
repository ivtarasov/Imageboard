using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Netaba.Data.Contexts;
using Netaba.Data.Entities;
using Netaba.Data.Services.Seeding;
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
            services.AddDbContext<UsersDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("UsersDbConnection")));

            services.AddDbContext<BoardsDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BoardsDbConnection")));

            services.Configure<UsersDbSeedingConfiguration>(Configuration.GetSection(nameof(UsersDbSeedingConfiguration)));

            Board[] boards = Configuration.GetSection(nameof(BoardsDbSeedingConfiguration)).Get<Board[]>();
            services.Configure<BoardsDbSeedingConfiguration>(bc => bc.Boards = boards);

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
