using Blog.Core.Utilities.Results.Abstract;
using Blog.Entites.ComplexTypes;
using Blog.Entites.DTOs.Image;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.UI.Helpers.Abstract
{
    public interface IImageHelper
    {
        Task<IDataResult<UploadedImageDto>> UploadImage(IFormFile pictureFile, string name, PictureTypeEnum pictureTypeEnum, string folderName = null);
        IDataResult<DeleteImageDto> DeleteImage(string pictureName);
    }
}
