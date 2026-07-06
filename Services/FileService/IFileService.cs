using Microsoft.AspNetCore.Http;

namespace Services.FileServices
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName, CancellationToken cancellationToken);
    }
}