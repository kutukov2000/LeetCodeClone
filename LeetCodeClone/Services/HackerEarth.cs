using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace LeetCodeClone
{
    class HackerEarth
    {
        private const string _codeEvaluationURL = "https://api.hackerearth.com/v4/partner/code-evaluation/submissions/";

        private readonly string _clientSecret;
        public HackerEarth(string clientSecret)
        {
            _clientSecret = clientSecret;
        }

        public async Task<string> ExecuteCodeAsync(HackerEarthRequestBody data)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("client-secret", _clientSecret);

                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(_codeEvaluationURL, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                dynamic responseObject = JObject.Parse(responseContent);

                return responseObject.he_id;
            }
        }
        public async Task<string> GetStatusAsync(string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("client-secret", _clientSecret);

                var response = await client.GetAsync(_codeEvaluationURL + id);
                var responseContent = await response.Content.ReadAsStringAsync();

                dynamic responseObject = JObject.Parse(responseContent);

                if (responseObject.request_status.code == "REQUEST_FAILED" || responseObject.result.compile_status != "OK")
                {
                    return "REQUEST_FAILED";
                }
                return responseObject.request_status.code;
            }
        }
        public async Task<HackerEarthOutputStats> GetStatsAsync(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("client-secret", _clientSecret);

                var response = await client.GetAsync(_codeEvaluationURL + id);
                var responseContent = await response.Content.ReadAsStringAsync();

                dynamic responseObject = JObject.Parse(responseContent);
                return new HackerEarthOutputStats
                {
                    MemoryUsed = responseObject.result.run_status.memory_used,
                    TimeUsed = responseObject.result.run_status.time_used,
                    DockPanelHeight = 40
                };
            }
        }
        public async Task<string> GetOutputStringAsync(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("client-secret", _clientSecret);

                var response = await client.GetAsync(_codeEvaluationURL + id);
                var responseContent = await response.Content.ReadAsStringAsync();

                dynamic responseObject = JObject.Parse(responseContent);
                return responseObject.result.run_status.output;
            }
        }
        public async Task<string> GetOutputAsync(string outputURL)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(outputURL);
                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
        }
    }
}