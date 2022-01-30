using ChatApp.Business.Factories;
using ChatApp.Business.Helpers.EntityHelpers;
using ChatApp.Cache;
using ChatApp.Db.AppDbContext;
using ChatApp.Dto;
using ChatApp.Extensions;
using ChatApp.UI._Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace ChatApp.UI
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
            services.AddControllersWithViews();
            services.AddDbContext<ChatAppDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSignalR();
            services.AddStackExchangeRedisCache(options => { options.Configuration = "localhost:6379"; });

            ServiceFactory.RegisterServices(services);
            services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(30));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            logger.AddFile($"{AppContext.BaseDirectory}/Logs/UI/mylog-{DateTime.Now:dd.MM.yyyy}.txt");
            FirstCache(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<ChatHub>("/chathub");
            });
        }

        public void FirstCache(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var serviceFactory = scope.ServiceProvider.GetRequiredService<ServiceFactory>();

            var allMessages = serviceFactory.MessageHelper.GetAll();
            serviceFactory.RedisCacheService.Set(CacheKeys.LastMessages, allMessages);
        }
    }
}
