using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Models;
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


        public async Task<List<string>> UploadAIModelImagesAsync(IEnumerable<IFormFile> images,string FolderName, CancellationToken cancellationToken)
        {
            var uploaded = new List<string>();

            foreach (var image in images)
            {
                var path = await SaveAsync(image, FolderName);
                uploaded.Add(path);
            }

            return uploaded;
        }

        public Task DeleteAIImagesAsync(IEnumerable<string> imagePaths)
        {
            foreach (var path in imagePaths)
            {
                if (File.Exists(path))
                    File.Delete(path);
            }

            return Task.CompletedTask;
        }
        private async Task<string> SaveAsync(IFormFile file,string FolderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            var uploadsFolder = Path.Combine(environment.WebRootPath, FolderName);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var fullPath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);

            }
            return fullPath;

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

        private string BuildReportFolder(string patientId, string reportId)
        {
            return Path.Combine(
                environment.WebRootPath,
                "uploads",
                "ai",
                patientId,
                reportId);
        }

    }
}
