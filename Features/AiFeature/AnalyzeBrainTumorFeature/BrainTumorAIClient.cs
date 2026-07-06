using Features.AiFeature.SharedMethod;
using Features.AiFeature.SharedMethod.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Features.AiFeature.AnalyzeBrainTumorFeature
{

    public interface IBrainTumorAIClient : IAIModelClient
    {
    }
    public class BrainTumorAIClient : BasePredictionAiClient, IBrainTumorAIClient
    {
        private readonly HttpClient httpClient;

        public BrainTumorAIClient(HttpClient httpClient) : base(httpClient, "https://fawzyshams009-brain-tumor-api.hf.space", "predict",
            "predict")
        {
            this.httpClient = httpClient;
        }

    }
}
