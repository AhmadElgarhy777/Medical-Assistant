using Features.AiFeature.SharedMethod.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Features.AiFeature.SharedMethod
{
    public class BasePredictionAiClient: IAIModelClient
    {
        protected readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _postApi;
        private readonly string _getApi;

        protected BasePredictionAiClient(
            HttpClient httpClient,
            string baseUrl,
            string postApi,
            string getApi)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl.TrimEnd('/');
            _postApi = postApi;
            _getApi = getApi;
        }

        // 1️⃣ Start Prediction
        public async Task<PredictionResult> PredictAsync(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("Local image file to analyze not found.", imagePath);
            }

            // Step 1: Upload the file to Gradio's upload endpoint
            string remoteFilePath;
            using (var form = new MultipartFormDataContent())
            {
                using (var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    using (var streamContent = new StreamContent(fileStream))
                    {
                        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                        form.Add(streamContent, "files", Path.GetFileName(imagePath));

                        var uploadResponse = await _httpClient.PostAsync(
                            $"{_baseUrl}/gradio_api/upload",
                            form);
                        uploadResponse.EnsureSuccessStatusCode();

                        var uploadResult = await uploadResponse.Content.ReadFromJsonAsync<List<string>>();
                        if (uploadResult == null || uploadResult.Count == 0)
                        {
                            throw new Exception("Failed to upload the image to the AI service.");
                        }
                        remoteFilePath = uploadResult[0];
                    }
                }
            }

            // Step 2: Trigger prediction with remote file path
            var predictPayload = new
            {
                data = new object[]
                {
                    new
                    {
                        path = remoteFilePath,
                        meta = new { _type = "gradio.FileData" }
                    }
                }
            };

            var predictResponse = await _httpClient.PostAsJsonAsync(
                $"{_baseUrl}/gradio_api/call/{_postApi}",
                predictPayload);

           
            predictResponse.EnsureSuccessStatusCode();

            var predictResult = await predictResponse.Content.ReadFromJsonAsync<PredictCallResponse>();
            if (predictResult == null || string.IsNullOrEmpty(predictResult.event_id))
            {
                throw new Exception("Failed to start prediction event on the AI service.");
            }
            string eventId = predictResult.event_id;

            // Step 3: Stream and parse Server-Sent Events (SSE) to get the result
            string streamUrl = $"{_baseUrl}/gradio_api/call/{_getApi}/{eventId}";
            using (var responseStream = await _httpClient.GetStreamAsync(streamUrl))
            {
                using (var reader = new StreamReader(responseStream))
                {
                    string lastEvent = null;
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (line.StartsWith("event:"))
                        {
                            lastEvent = line.Substring("event:".Length).Trim();
                        }
                        else if (line.StartsWith("data:") && lastEvent == "complete")
                        {
                            string dataJson = line.Substring("data:".Length).Trim();
                            var results = JsonSerializer.Deserialize<List<HfResponse>>(dataJson);
                            if (results != null && results.Count > 0)
                            {
                                return new PredictionResult
                                {
                                    Prediction = results[0].prediction,
                                    Confidence = results[0].confidence
                                };
                            }
                        }
                    }
                }
            }

            throw new Exception("AI model stream completed without returning a prediction result.");
        }

        private class PredictCallResponse
        {
            public string event_id { get; set; } = null!;
        }

        private class HfResponse
        {
            public string prediction { get; set; } = null!;
            public double confidence { get; set; }
        }
    }

}
