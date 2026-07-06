using Features.AiFeature.SharedMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AiFeature.SkinCancerClassification
{
   public interface ISkinCancerClassificationAIClient : IAIModelClient
    {
    }
    public class SkinCancerClassificationAIClient : BasePredictionAiClient, ISkinCancerClassificationAIClient
    {
        private readonly HttpClient httpClient;

        public SkinCancerClassificationAIClient(HttpClient httpClient) : base(httpClient, "https://fawzyshams009-skin-cancer-classification.hf.space", "predict",
            "predict")
        {
            this.httpClient = httpClient;
        }

    }
}
