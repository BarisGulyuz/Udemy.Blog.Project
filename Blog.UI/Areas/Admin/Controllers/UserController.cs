using AutoMapper;
using Blog.Core.Utilities.Extensions;
using Blog.Entites.ComplexTypes;
using Blog.Entites.Concrete;
using Blog.Entites.DTOs;
using Blog.UI.Areas.Admin.Models;
using Blog.UI.Helper;
using Blog.UI.Helpers.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blog.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Edıtor")]
    public class UserController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IImageHelper _ımageHelper;
        public UserController(UserManager<User> userManager, IWebHostEnvironment env, IMapper mapper, SignInManager<User> signInManager, IImageHelper ımageHelper, IToastNotification toastNotification)
        {
            _userManager = userManager;
            _env = env;
            _mapper = mapper;
            _signInManager = signInManager;
            _ımageHelper = ımageHelper;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(new UserListDto
            {
                Users = users,
                ResultStatus = Core.Utilities.Results.ResultStatus.Success
            });

        }
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_UserAddPartial");
        }
        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            if (ModelState.IsValid)
            {
                //FileUploadHelper fileUploadHelper = new(_env);
                //string imagePath = await fileUploadHelper.ImageUpload(userAddDto.PictureFile, userAddDto.UserName);
                var imagePath = await _ımageHelper.UploadImage(userAddDto.PictureFile, userAddDto.UserName, PictureTypeEnum.User, "userImages");
                userAddDto.Picture = imagePath.ResultStatus == Core.Utilities.Results.ResultStatus.Success ? imagePath.Data.FullName : "user-image-defualt.png";

                var user = _mapper.Map<User>(userAddDto);
                var result = await _userManager.CreateAsync(user, userAddDto.Password);
                if (result.Succeeded)
                {
                    var categoryModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = Core.Utilities.Results.ResultStatus.Success,
                            Messages = $"{user.UserName} adlı kullanıcı başarıyla eklenmiştir",
                            User = user,
                        },
                        UserPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(categoryModel);
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                    var userErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserAddDto = userAddDto,
                        UserPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userErrorModel);
                }
            }
            var userErrorModels = JsonSerializer.Serialize(new UserAddAjaxViewModel
            {
                UserAddDto = userAddDto,
                UserPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
            });
            return Json(userErrorModels);
        }

        public async Task<JsonResult> Delete(int userId)
        {
            string message = "Kullanıcı bulunamadı";
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    var deletedUser = JsonSerializer.Serialize(new UserDto
                    {
                        ResultStatus = Core.Utilities.Results.ResultStatus.Success,
                        Messages = $"{user.UserName} adlı kullanıcı silindi"
                    });
                    return Json(deletedUser);
                }
                else
                {
                    string errorMessages = String.Empty;
                    foreach (var error in result.Errors)
                    {
                        errorMessages = $"{error.Description}\n";
                    }
                    var deletedUserError = JsonSerializer.Serialize(new UserDto
                    {
                        User = user,
                        Messages = $"{user.UserName} adlı kullanıcı silinirken bir hata oluştu...\n ${errorMessages}"
                    });
                    return Json(deletedUserError);
                }
            }
            return Json(message);
        }
        [HttpGet]
        public async Task<PartialViewResult> Update(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var userDto = _mapper.Map<UserUpdateDto>(user);
            return PartialView("_UserUpdatePartial", userDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            //FileUploadHelper fileUploadHelper = new(_env);
            if (ModelState.IsValid)
            {
                bool isNewPicUpload = false;

                var oldUser = await _userManager.FindByIdAsync(userUpdateDto.Id.ToString());
                var oldPicture = oldUser.Picture;
                if (userUpdateDto.PictureFile != null)
                {
                    //userUpdateDto.Picture = await fileUploadHelper.ImageUpload(userUpdateDto.PictureFile, userUpdateDto.UserName);
                    var imagePath = await _ımageHelper.UploadImage(userUpdateDto.PictureFile, userUpdateDto.UserName, PictureTypeEnum.User, "userImages");
                    userUpdateDto.Picture = imagePath.ResultStatus == Core.Utilities.Results.ResultStatus.Success ? imagePath.Data.FullName : "user-image-defualt.png";
                    isNewPicUpload = true;
                }
                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isNewPicUpload)
                    {
                        //fileUploadHelper.ImageDelete(oldPicture);
                        _ımageHelper.DeleteImage(oldPicture);
                    }
                    var userModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = Core.Utilities.Results.ResultStatus.Success,
                            Messages = $"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir",
                            User = updatedUser
                        },
                        UserPartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                    });
                    return Json(userModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    var userErrorModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserUpdateDto = userUpdateDto,
                        UserPartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                    });
                    return Json(userErrorModel);
                }
            }
            var userErrorModelState = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
            {
                UserUpdateDto = userUpdateDto,
                UserPartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
            });
            return Json(userErrorModelState);
        }

        [HttpGet]
        public async Task<ViewResult> ChangeDetails()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var updateDto = _mapper.Map<UserUpdateDto>(user);
            return View(updateDto);
        }
        [HttpPost]
        public async Task<ViewResult> ChangeDetails(UserUpdateDto userUpdateDto)
        {
            //FileUploadHelper fileUploadHelper = new(_env);
            if (ModelState.IsValid)
            {
                bool isNewPicUpload = false;

                var oldUser = await _userManager.GetUserAsync(HttpContext.User);
                var oldPicture = oldUser.Picture;
                if (userUpdateDto.PictureFile != null)
                {
                    //userUpdateDto.Picture = await fileUploadHelper.ImageUpload(userUpdateDto.PictureFile, userUpdateDto.UserName);
                    var imagePath = await _ımageHelper.UploadImage(userUpdateDto.PictureFile, userUpdateDto.UserName, PictureTypeEnum.User, "userImages");
                    userUpdateDto.Picture = imagePath.ResultStatus == Core.Utilities.Results.ResultStatus.Success ? imagePath.Data.FullName : "user-image-defualt.png";
                    isNewPicUpload = true;
                }
                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isNewPicUpload)
                    {
                        //fileUploadHelper.ImageDelete(oldPicture);
                        _ımageHelper.DeleteImage(oldPicture);
                    }
                    //TempData.Add("SuccessMessage", "Bilgiler Başarıyla Güncellendi");
                    _toastNotification.AddSuccessToastMessage("Bilgiler Başarıyla Güncellendi");
                    return View(userUpdateDto);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(userUpdateDto);
                }
            }
            return View(userUpdateDto);
        }
        [HttpGet]
        public ViewResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserPasswordChangeDto userPasswordChangeDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var ısVerified = await _userManager.CheckPasswordAsync(user, userPasswordChangeDto.CurrentPassword);
                if (ısVerified)
                {
                    var result = await _userManager.ChangePasswordAsync(user, userPasswordChangeDto.CurrentPassword, userPasswordChangeDto.NewPassword);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(user);
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(user, userPasswordChangeDto.NewPassword, true, false);
                        //TempData.Add("SuccessMessage", "Şifreniz Başarıyla Güncellendi");
                        _toastNotification.AddSuccessToastMessage("Bilgiler Başarıyla Güncellendi");
                        return View(userPasswordChangeDto);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(userPasswordChangeDto);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Mevcut şifrenizi yanlış girdiniz");
                    return View(userPasswordChangeDto);
                }

            }
            return View(userPasswordChangeDto);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, userLoginDto.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "E-posta adresiniz veya şifreniz yanlıştır");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-posta adresiniz veya şifreniz yanlıştır");
                    return View();
                }
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}

