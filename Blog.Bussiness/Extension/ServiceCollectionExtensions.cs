using Blog.Bussiness.Abstract;
using Blog.Bussiness.Concrete;
using Blog.DataAccess.Concrete.Contexts;
using Blog.DataAccess.UnitOfWork;
using Blog.Entites.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Bussiness.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<BlogContext>(opt => opt.UseSqlServer(connectionString));
            services.AddIdentity<User, Role>(opt =>
            {
                //User-Password Options
                opt.Password.RequireDigit = false;   //rakam bulunmalı mı?
                opt.Password.RequiredLength = 8;
                opt.Password.RequiredUniqueChars = 0; //özel karakter bulundurmalı mı?
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;

                //Username-Email Options
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                opt.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<BlogContext>();


            services.AddScoped<IUnitofWork, UnitOfWork>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IArticleService, ArticleManager>();
            services.AddScoped<ICommentService, CommentManager>();

            return services;
        }
    }
}
