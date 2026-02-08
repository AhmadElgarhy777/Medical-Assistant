using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImageServices
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment environment;

        public ImageService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public async Task<string> EditImgAsync(IFormFile Newimg, string OldImg, string FolderName, CancellationToken cancellationToken)
        {
            ImgExtention(Newimg);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Newimg.FileName);
            var path = Path.Combine(environment.WebRootPath, FolderName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fullPath = Path.Combine(path, fileName);
            using var stream = new FileStream(fullPath, FileMode.Create);
            await Newimg.CopyToAsync(stream, cancellationToken);

            if (!string.IsNullOrWhiteSpace(OldImg))
            {
                var oldFileName=Path.GetFileName(OldImg);
                var oldFullPath=Path.Combine(path, oldFileName);
                if (File.Exists(oldFullPath))
                    File.Delete(oldFullPath);
            }
            return fileName;

        }

        public async Task<string> UploadImgAsync(IFormFile img, string FolderName, CancellationToken cancellationToken)
        {

            ImgExtention(img);

            var fileName= Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
            var path = Path.Combine(environment.WebRootPath, FolderName);

            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fullPath= Path.Combine(path, fileName);
            using var stream = new FileStream(fullPath, FileMode.Create);
            await img.CopyToAsync(stream,cancellationToken);

            return fileName;

        }

        public void ImgExtention(IFormFile image)
        {
            var extention = Path.GetExtension(image.FileName);
            string[] ext = { ".jpg", ".png", ".jpeg" };

            if (!ext.Contains(extention))
                throw new Exception("Invalid image type");

            if (image.Length > 5 * 1024 * 1024)
                throw new Exception("Image is Large , Max is 5 Mbyte");
        }
    }
}
