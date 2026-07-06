using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Services.ImageServices.Dtos;

namespace Services.ImageServices
{
    public interface IImageQualityService
    {
        Task<ImageQualityResultDto> ValidateImageAsync(Stream stream, string fileExtension, CancellationToken cancellationToken = default);
    }
}
