using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models;
using Models.Enums;

namespace Features.AiFeature.SharedMethod
{
    public interface IAiAnalysisOrchestrator
    {
        Task<ResultResponse<AiReport>> ProcessAnalysisAsync(
            List<string> imagePaths,
            string patientId,
            string doctorId,
            string? doctorNote,
            AiModelTypeEnum modelType,
            CancellationToken cancellationToken);
    }
}
