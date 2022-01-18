using Blog.Bussiness.Abstract;
using Blog.UI.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.UI.Controllers
{
    public class ArticleController : Controller
    {
        IArticleService _articleService;
        private readonly IToastNotification _toastNotification;

        public ArticleController(IArticleService articleService, IToastNotification toastNotification)
        {
            _articleService = articleService;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int articleId)
        {
            var article = await _articleService.GetArticle(articleId);
            if (article.ResultStatus == Core.Utilities.Results.ResultStatus.Success)
            {
                return View(article.Data);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            var searchResult = await _articleService.Search(keyword, currentPage, pageSize, isAscending);
            if (searchResult.ResultStatus == Core.Utilities.Results.ResultStatus.Success)
            {
                return View(new ArticlesSearchVM
                {
                    ArticleListDto = searchResult.Data,
                    Keyword = keyword
                });
            }
            return NotFound();
        }
    }
}
