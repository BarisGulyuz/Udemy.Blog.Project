using AutoMapper;
using Blog.Bussiness.Abstract;
using Blog.Core.Utilities.Extensions;
using Blog.Entites.Concrete;
using Blog.Entites.DTOs;
using Blog.UI.Areas.Admin.Models;
using Blog.UI.Helpers.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blog.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Edıtor")]
    public class CategoryController : BaseController
    {
        ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, UserManager<User> userManager, IMapper mapper, IImageHelper ımageHelper) : base(userManager, mapper, ımageHelper)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllbyDeletedandActive();
            return View(categories.Data);
        }

        public async Task<JsonResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllbyDeletedandActive();
            var categories = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            });
            return Json(categories);

        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_CategoryAddPartial");
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)
        {
            if (!ModelState.IsValid)
            {
                var categoryErrorModel = JsonSerializer.Serialize(new CategoryAddAjaxModel
                {
                    CategoryPartial = await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto)
                });
                return Json(categoryAddDto);
            }
            var newCategory = await _categoryService.Add(categoryAddDto, LoggedInUser.UserName);
            if (newCategory.ResultStatus == Core.Utilities.Results.ResultStatus.Success)
            {
                var categoryModel = JsonSerializer.Serialize(new CategoryAddAjaxModel
                {
                    CategoryDto = newCategory.Data,
                    CategoryPartial = await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto),
                });
                return Json(categoryModel);
            }
            return Json(categoryAddDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int categoryId)
        {
            var category = await _categoryService.GetCategoryUpdateDto(categoryId);
            if (category.ResultStatus == Core.Utilities.Results.ResultStatus.Success)
            {
                return PartialView("_CategoryUpdatePartial", category.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var newCategory = await _categoryService.Update(categoryUpdateDto, LoggedInUser.UserName);
                if (newCategory.ResultStatus == Core.Utilities.Results.ResultStatus.Success)
                {
                    var categoryModel = JsonSerializer.Serialize(new CategoryUpdateAjaxModel
                    {
                        CategoryDto = newCategory.Data,
                        CategoryPartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto),
                    });
                    return Json(categoryModel);
                }
            }
            var categoryErrorModel = JsonSerializer.Serialize(new CategoryUpdateAjaxModel
            {
                CategoryPartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto)
            });
            return Json(categoryErrorModel);
        }


        [HttpPost]
        public async Task<JsonResult> Delete(int categoryId)
        {
            var result = await _categoryService.Delete(categoryId, LoggedInUser.UserName);
            var deletedCategory = JsonSerializer.Serialize(result);
            return Json(deletedCategory);
        }
    }
}
