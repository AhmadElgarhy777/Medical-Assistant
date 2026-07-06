using Features.AiFeature.SharedMethod.Dtos;
using Features.AiService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AiFeature.SharedMethod
{
    public interface IAIModelClient
    {
        Task<PredictionResult> PredictAsync(string imagePath);
    }
}
