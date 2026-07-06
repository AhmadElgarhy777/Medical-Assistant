using Features.AiFeature.SharedMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AiFeature.CBCBloodTest
{
    public interface ICBCBloodTestAiClient : IAIModelClient
    {
    }
    public class CBCBloodTestAiClient : BasePredictionAiClient, ICBCBloodTestAiClient
    {
        private readonly HttpClient httpClient;

        public CBCBloodTestAiClient(HttpClient httpClient) : base(httpClient, "https://MahmoudRdad-CBC-Test-Diagnosis.hf.space", "v2/analyze_cbc",
            "analyze_cbc")
        {
            this.httpClient = httpClient;
        }

    }
}
