using Blog.Bussiness.Abstract;
using Blog.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.UI.ViewComponents
{
    public class RightSideNavViewComponent: ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;

        public RightSideNavViewComponent(ICategoryService categoryService, IArticleService articleService)
        {
            _categoryService = categoryService;
            _articleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetAllbyDeletedandActive();
            var articles = await _articleService.GetAllByViewCount(false, 5);
            return View(new RightSideNavVM
            {
                Categories = categories.Data.Categories,
                Articles = articles.Data.Articles
            });
        }
    }
}
