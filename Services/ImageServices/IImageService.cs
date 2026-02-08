using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImageServices
{
    public interface IImageService
    {
       Task<string> UploadImgAsync(IFormFile img, string FolderName, CancellationToken cancellationToken);
       Task<string> EditImgAsync(IFormFile Newimg,string OldImg, string FolderName, CancellationToken cancellationToken);
    }
}
