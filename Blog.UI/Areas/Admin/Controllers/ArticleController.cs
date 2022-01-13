using AutoMapper;
using Blog.Bussiness.Abstract;
using Blog.Entites.Concrete;
using Blog.Entites.DTOs;
using Blog.UI.Areas.Admin.Models;
using Blog.UI.Helpers.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blog.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : BaseController
    {
        private readonly IToastNotification _toastNotification;
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;

        public ArticleController(IArticleService articleService, ICategoryService categoryService, UserManager<User> userManager, IMapper mapper, IImageHelper ımageHelper, IToastNotification toastNotification) : base(userManager, mapper, ımageHelper)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllbyDeletedandActive();
            if (articles.ResultStatus == Core.Utilities.Results.ResultStatus.Success)
                return View(articles.Data);
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryService.GetAllbyDeletedandActive();
            if (categories.ResultStatus == Core.Utilities.Results.ResultStatus.Success)
            {
                return View(new ArticleAddViewModel
                {
                    Categories = categories.Data.Categories
                });

            }
            //TempData.Add("Nocategory", "Kategori kaydı mevcut değil");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(ArticleAddViewModel articleAddViewModel)
        {
            var categories = await _categoryService.GetAllbyDeletedandActive();
            articleAddViewModel.Categories = categories.Data.Categories;

            if (ModelState.IsValid)
            {
                var articleDto = Mapper.Map<ArticleAddDto>(articleAddViewModel);
                var thumbnail = await ImageHelper.UploadImage(articleAddViewModel.TumbnailFile, articleAddViewModel.Title, Entites.ComplexTypes.PictureTypeEnum.Article, "articleImages");
                articleDto.Tumbnail = thumbnail.Data.FullName;
                var result = await _articleService.Add(articleDto, LoggedInUser.UserName, LoggedInUser.Id);
                if (result.ResultStatus == Core.Utilities.Results.ResultStatus.Success)
                {
                    _toastNotification.AddSuccessToastMessage(result.Message);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }

            }
            return View(articleAddViewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Update(int articleId)
        {
            var article = await _articleService.GetArticleUpdateDto(articleId);
            var categories = await _categoryService.GetAllbyDeletedandActive();
            if (article.ResultStatus == Core.Utilities.Results.ResultStatus.Success && categories.ResultStatus == Core.Utilities.Results.ResultStatus.Success)
            {
                var artictleVM = Mapper.Map<ArticleUpdateViewModel>(article.Data);
                artictleVM.Categories = categories.Data.Categories;
                return View(artictleVM);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(ArticleUpdateViewModel articleUpdateViewModel)
        {
            if (ModelState.IsValid)
            {
                bool isThumbnailUpload = false;
                var oldhtumbnail = articleUpdateViewModel.Tumbnail;
                if (articleUpdateViewModel.TumbnailFile != null)
                {
                    var uploadImage = await ImageHelper.UploadImage(articleUpdateViewModel.TumbnailFile, articleUpdateViewModel.Title, Entites.ComplexTypes.PictureTypeEnum.Article, "articleImages");
                    if (uploadImage.ResultStatus == Core.Utilities.Results.ResultStatus.Success)
                    {
                        articleUpdateViewModel.Tumbnail = uploadImage.Data.FullName;
                        isThumbnailUpload = true;
                    }
                }
                var artictleUpdateDto = Mapper.Map<ArticleUpdateDto>(articleUpdateViewModel);
                var result = await _articleService.Update(artictleUpdateDto, LoggedInUser.UserName);
                if (result.ResultStatus == Core.Utilities.Results.ResultStatus.Success)
                {
                    if (isThumbnailUpload == true)
                    {
                        ImageHelper.DeleteImage(oldhtumbnail);
                    }
                    //TempData.Add("SuccessMessage", "Bilgiler Başarıyla Güncellendi");
                    _toastNotification.AddSuccessToastMessage("Bilgiler Başarıyla Güncellendi", new ToastrOptions
                    {
                        CloseButton = true,
                        Title = "İşlem Başarılı"
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }

            }
            var categories = await _categoryService.GetAllbyDeletedandActive();
            articleUpdateViewModel.Categories = categories.Data.Categories;
            return View(articleUpdateViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int articleId)
        {
            var result = await _articleService.Delete(articleId, LoggedInUser.UserName);
            var articleResult = JsonSerializer.Serialize(result);
            return Json(articleResult);

        }
    }
}
