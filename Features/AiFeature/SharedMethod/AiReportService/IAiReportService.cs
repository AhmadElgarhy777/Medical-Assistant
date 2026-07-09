using Models;
using Models.DTOs.AiServicesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AiFeature.SharedMethod.AiReportService
{
    public interface IAiReportService
    {
        Task<AiReportResultDto> AnalyzeAndSaveAsync(
            List<string> imagePaths,
            AiModelTypeEnum modelType,
            string patientId,
            string? doctorId,
            string? doctorNote,
            CancellationToken cancellationToken);
    }
}
