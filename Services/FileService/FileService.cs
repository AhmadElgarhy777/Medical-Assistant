using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Services.FileServices
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName, CancellationToken cancellationToken)
        {
            var allowedExtensions = new[]
            {
                // صور
                ".jpg", ".jpeg", ".png", ".gif",
                // ملفات
                ".pdf", ".doc", ".docx", ".txt", ".xlsx",
                // صوت
                ".mp3", ".wav", ".ogg", ".m4a", ".aac"
            };

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new Exception("نوع الملف مش مسموح بيه!");

            if (file.Length > 20 * 1024 * 1024)
                throw new Exception("الملف كبير جداً! الحد الأقصى 20 MB");

            var fileName = Guid.NewGuid().ToString() + extension;
            var path = Path.Combine(_environment.WebRootPath, folderName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fullPath = Path.Combine(path, fileName);
            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            return fileName;
        }
    }
}