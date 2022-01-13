using Blog.Bussiness.Abstract;
using Blog.Entites.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Utilities.Results;
using Blog.UI.Areas.Admin.Models;

namespace Blog.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Edıtor")]
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public HomeController(ICategoryService categoryService, IArticleService articleService, ICommentService commentService, UserManager<User> userManager)
        {
            _categoryService = categoryService;
            _articleService = articleService;
            _commentService = commentService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var categoriesCount = await _categoryService.CountbyNoneDeleted();
            var commentCount = await _commentService.CountbyNoneDeleted();
            var articlesCount = await _articleService.CountbyNoneDeleted();
            var articles = await _articleService.GetAllbyDeletedandActive();
            var usesrCount = await _userManager.Users.CountAsync();
            if (categoriesCount.ResultStatus == ResultStatus.Success && commentCount.ResultStatus == ResultStatus.Success && articlesCount.ResultStatus == ResultStatus.Success && articles.ResultStatus == ResultStatus.Success && usesrCount > -1)
            {
                return View(new DashboardViewModel
                {
                    CatagoriesCount = categoriesCount.Data,
                    CommentsCount = commentCount.Data,
                    ArticlesCount = articlesCount.Data,
                    UsersCount = usesrCount,
                    Articles = articles.Data
                });
            }
            return NotFound();
        }
    }
}
