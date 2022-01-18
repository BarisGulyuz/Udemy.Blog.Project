using Blog.Bussiness.Abstract;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.UI.Controllers
{

    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IToastNotification _toastNotification;
        public HomeController(IArticleService articleService, IToastNotification toastNotification)
        {
            _articleService = articleService;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            if (categoryId == null)
            {
                var articles = await _articleService.GetAllbyPages(null, currentPage, pageSize, isAscending);
                if (articles.ResultStatus == Core.Utilities.Results.ResultStatus.Success)
                {
                    return View(articles.Data);
                }
            }
            else
            {
                var articles = await _articleService.GetAllbyPages(categoryId.Value, currentPage, pageSize, isAscending);
                if (articles.ResultStatus == Core.Utilities.Results.ResultStatus.Success)
                {
                    if (articles.Data.Articles.Count == 0) _toastNotification.AddErrorToastMessage("Makale bulunamadı");
                    else _toastNotification.AddSuccessToastMessage("Makaleler başarıyla yüklendi");
                    return View(articles.Data);
                }

            }
            _toastNotification.AddErrorToastMessage("Makaleler yüklenirken bir hata oluştu");
            return NotFound();
        }
    }
}
