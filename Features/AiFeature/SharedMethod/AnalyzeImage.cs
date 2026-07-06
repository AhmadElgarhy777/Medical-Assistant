using Features.AiFeature.SharedMethod.Dtos;
using Models.DTOs.AiServicesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AiFeature.SharedMethod
{
    public interface IAnalyzeImage
    {
        Task<AiAnalysisResultDTO> AnalyzeImagesAsync(List<string> imagePaths, IAIModelClient modelClient, CancellationToken cancellationToken);
    }
    public class AnalyzeImage:IAnalyzeImage
    {
        
        public async Task<AiAnalysisResultDTO> AnalyzeImagesAsync(
    List<string> imagePaths,
    IAIModelClient modelClient,
    CancellationToken cancellationToken)
        {
            var predictions = new List<PredictionResult>();

            foreach (var imagePath in imagePaths)
            {
                var prediction = await modelClient.PredictAsync(imagePath);
                predictions.Add(prediction);
            }

            var FinalPrediction = predictions
        .GroupBy(p => p.Prediction)
        .OrderByDescending(g => g.Count())
        .First()
        .Key;

            var averageConfidence = predictions
                .Average(p => p.Confidence);

            return new AiAnalysisResultDTO
            {
                Prediction = FinalPrediction,
                Confidence = averageConfidence
            };
        }

    }
}
