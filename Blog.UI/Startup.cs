using Blog.Bussiness.AutoMapper.Profiles;
using Blog.Bussiness.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Blog.UI.AutoMapper.Profiles;
using Blog.UI.Helpers.Abstract;
using Blog.UI.Helpers.Concrete;

namespace Blog.UI
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

            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            }).AddNToastNotifyToastr();

            services.AddSession();

            //AutoMapper
            services.AddAutoMapper(typeof(CategoryProfille), typeof(ArticleProfile), typeof(UserProfile), typeof(ViewModelsProfiles), typeof(CommentProfile));
            //Servies and DependencyResolver
            services.LoadServices(Configuration.GetConnectionString("BlogDb"));
            services.AddScoped<IImageHelper, ImageHelper>();

            //Identity Cofniguration
            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = new PathString("/Admin/User/Login");
                opt.LogoutPath = new PathString("/Admin/User/Logout");
                opt.Cookie = new CookieBuilder
                {
                    Name = "Udemy-Blog",
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest, //Always
                };
                opt.SlidingExpiration = true;
                opt.ExpireTimeSpan = System.TimeSpan.FromDays(7);
                opt.AccessDeniedPath = new PathString("/Admin/User/AccessDenied");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseNToastNotify();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                     name: "areas",
                     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
