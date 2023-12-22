using HackerEarthApi.JsonModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace LeetCodeClone
{
    public static class HackerEarth
    {
        private const string _codeEvaluationURL = "https://api.hackerearth.com/v4/partner/code-evaluation/submissions/";
        private const string _clientSecret = "acc5789876823ef7de80d3a2dd8892bed9cc1b35 ";

        private static readonly HttpClient _httpClient;

        static HackerEarth()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("client-secret", _clientSecret);
        }

        public static async Task<string?> ExecuteCodeAsync(RequestBody data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.PostAsync(_codeEvaluationURL, content);

            var responseContent = await HandleResponse(response);
            return responseContent.Id;
        }

        public static async Task<HackerEarthApiOutput> GetOutputStatsAsync(string id)
        {
            var response = await _httpClient.GetAsync(_codeEvaluationURL + id);

            return await HandleResponse(response);
        }

        public static async Task<string> GetOutputStringAsync(string outputURL)
        {
            var response = await _httpClient.GetAsync(outputURL);
            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        private static async Task<HackerEarthApiOutput> HandleResponse(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(JsonConvert.DeserializeObject<ErrorMessage>(responseContent).Message);
            }

            return JsonConvert.DeserializeObject<HackerEarthApiOutput>(responseContent);
        }
    }
}