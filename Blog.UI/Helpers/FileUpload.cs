using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Blog.Core.Utilities.Extensions;
using Microsoft.AspNetCore.Hosting;

namespace Blog.UI.Helper
{
    public class FileUploadHelper
    {

        private readonly IWebHostEnvironment _env;

        public FileUploadHelper(IWebHostEnvironment env)
        {
            _env = env;
        }
        public FileUploadHelper()
        {

        }
        public async Task<string> ImageUpload(IFormFile formFile, string name)
        {
            var pictureFile = formFile.FileName;
            string wwwroot = _env.WebRootPath;
            DateTime dateTime = DateTime.Now;

            string fileExtension = Path.GetExtension(pictureFile);
            string fileName = $"{name}_{dateTime.DateTimeStringWihtUnderScore()}{fileExtension}";
            var path = Path.Combine($"{wwwroot}/images", fileName);
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            return fileName;
        }
        
        public bool ImageDelete(string imageName)
        {
            string wwwroot = _env.WebRootPath;
            var fileToDelete = Path.Combine($"{wwwroot}/images", imageName);
            if (File.Exists(fileToDelete))
            {
                File.Delete(fileToDelete);
                return true;
            }
            return false;
        }
    }
}
