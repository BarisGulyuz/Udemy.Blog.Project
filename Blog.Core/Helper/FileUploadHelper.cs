//using Microsoft.AspNetCore.Http;
//using System;
//using System.IO;
//using System.Threading.Tasks;
//using Blog.Core.Utilities.Extensions;


//namespace Blog.Core.Helper
//{
//    public class FileUploadHelper
//    {

//        private readonly IWebHostEnvironment _env;

//        public FileUploadHelper(IWebHostEnvironment env)
//        {
//            _env = env;
//        }

//        public async Task<string> ImageUpload(IFormFile formFile, string name)
//        {
//            var pictureFile = formFile.FileName;
//            string wwwroot = _env.WebRootPath;
//            DateTime dateTime = DateTime.Now;

//            string fileExtension = Path.GetExtension(pictureFile);
//            string fileName = $"{name}_{dateTime.DateTimeStringWihtUnderScore()}{fileExtension}";
//            var path = Path.Combine($"{wwwroot}/images", fileName);
//            await using (var stream = new FileStream(path, FileMode.Create))
//            {
//                await formFile.CopyToAsync(stream);
//            }
//            return fileName;
//        }
//    }
//}
