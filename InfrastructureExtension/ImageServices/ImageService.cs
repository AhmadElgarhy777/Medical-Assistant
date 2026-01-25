using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureExtension.ImageServices
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment environment;

        public ImageService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        public async Task<string> UploadImgAsync(IFormFile img, string FolderName, CancellationToken cancellationToken)
        {
            var extention=Path.GetExtension(img.FileName);
            string[] ext = { ".jpg", ".png", ".jpeg" };

            if(!ext.Contains(extention))
                throw new Exception("Invalid image type");

            if(img.Length>5 * 1024 * 1024)
                throw new Exception("Image is Large , Max is 5 Mbyte");

            var fileName= Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
            var path = Path.Combine(environment.WebRootPath, FolderName);

            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fullPath= Path.Combine(path, fileName);
            using var stream = new FileStream(fullPath, FileMode.Create);
            await img.CopyToAsync(stream,cancellationToken);

            return fileName;

        }
    }
}
