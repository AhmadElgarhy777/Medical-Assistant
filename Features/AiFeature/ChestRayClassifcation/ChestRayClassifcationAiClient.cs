using Features.AiFeature.SharedMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AiFeature.ChestRayClassifcation
{
   public interface IChestRayClassifcationAiClient : IAIModelClient
    {
    }
    public class ChestRayClassifcationAiClient : BasePredictionAiClient, IChestRayClassifcationAiClient
    {
        private readonly HttpClient httpClient;

        public ChestRayClassifcationAiClient(HttpClient httpClient) : base(httpClient, "https://fawzyshams009-chest-ray-classification.hf.space", "predict",
            "predict")
        {
            this.httpClient = httpClient;
        }

    }
}
