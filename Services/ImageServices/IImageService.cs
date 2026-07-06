using Microsoft.AspNetCore.Http;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Services.ImageServices.ImageService;

namespace Services.ImageServices
{
    public interface IImageService
    {
       Task<string> UploadImgAsync(IFormFile img, string FolderName, CancellationToken cancellationToken);
       Task<string> EditImgAsync(IFormFile Newimg,string OldImg, string FolderName, CancellationToken cancellationToken);
        void ImgExtention(IFormFile image);
        Task<List<string>> UploadAIModelImagesAsync(IEnumerable<IFormFile> images, CancellationToken cancellationToken);
    }
}
