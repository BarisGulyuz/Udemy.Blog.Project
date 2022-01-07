using Blog.Bussiness.Abstract;
using Blog.Bussiness.Concrete;
using Blog.DataAccess.Concrete.Contexts;
using Blog.DataAccess.UnitOfWork;
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
        public static IServiceCollection LoadServices(this IServiceCollection services)
        {
            services.AddDbContext<BlogContext>();

            services.AddScoped<IUnitofWork, UnitOfWork>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IArticleService, ArticleManager>();
            return services;
        }
    }
}
