using Blog.Core.Utilities.Extensions;
using Blog.Core.Utilities.Results.Abstract;
using Blog.Core.Utilities.Results.Concrete;
using Blog.Entites.ComplexTypes;
using Blog.Entites.DTOs.Image;
using Blog.UI.Helpers.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.UI.Helpers.Concrete
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _wwwrooot;
        private readonly string imagesFolder = "images";

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
            _wwwrooot = _env.WebRootPath;
        }

        public IDataResult<DeleteImageDto> DeleteImage(string pictureName)
        {
            var fileToDelete = Path.Combine($"{_wwwrooot}/{imagesFolder}", pictureName);
            if (File.Exists(fileToDelete))
            {
                var fileInfo = new FileInfo(fileToDelete);
                var imagesDeletedInfo = new DeleteImageDto
                {
                    FullName = pictureName,
                    Extension = fileInfo.Extension,
                    Path = fileInfo.FullName,
                    Size = fileInfo.Length
                };
                File.Delete(fileToDelete);
                return new DataResult<DeleteImageDto>(Core.Utilities.Results.ResultStatus.Success, imagesDeletedInfo);
            }
            return new DataResult<DeleteImageDto>(Core.Utilities.Results.ResultStatus.Error, "Görsel silinemedi", null);
        }

        public async Task<IDataResult<UploadedImageDto>> UploadImage(IFormFile pictureFile, string name, PictureTypeEnum pictureTypeEnum, string folderName = null)
        {
            //folderName ??= pictureTypeEnum == PictureTypeEnum.User ? "userImages" : "articleImages";

            string newName = name.Replace(" ", "_");

            if (!Directory.Exists($"{_wwwrooot}/{imagesFolder}/{folderName}"))
            {
                Directory.CreateDirectory($"{_wwwrooot}/{imagesFolder}/{folderName}");
            }

            var oldFileName = pictureFile.FileName;
            DateTime dateTime = DateTime.Now;

            string fileExtension = Path.GetExtension(oldFileName);
            string newfileName = $"{name.Replace(' ', '_').Substring(0,6)}_{dateTime.DateTimeStringWihtUnderScore()}{fileExtension}";
            var path = Path.Combine($"{_wwwrooot}/{imagesFolder}/{folderName}", newfileName);
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }
            string type =  pictureTypeEnum == PictureTypeEnum.User ? "kullancıya" : "makaleye";

            return new DataResult<UploadedImageDto>(Core.Utilities.Results.ResultStatus.Success, $"{name} adlı {type} ait fotoğraf eklenmiştir.", new UploadedImageDto
            {
                FullName = $"{folderName}/{newfileName}",
                OldName = oldFileName,
                Extension = fileExtension,
                FolderName = folderName,
                Path = path,
                Size = pictureFile.Length
            });
        }
    }
}
