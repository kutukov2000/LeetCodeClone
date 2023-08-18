﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;

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

        public string Execute(RequestBody data)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("client-secret", _clientSecret);

                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                var response = client.PostAsync(_codeEvaluationURL, content).Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;

                dynamic responseObject = JObject.Parse(responseContent);

                return responseObject.he_id;
            }
        }
        public (string, string) GetStatus(string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("client-secret", _clientSecret);

                var response = client.GetAsync(_codeEvaluationURL + id).Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;

                dynamic responseObject = JObject.Parse(responseContent);
                MessageBox.Show($"responseObject {responseObject}");
                if (responseObject != null && responseObject.request_status != null)
                {
                    if (responseObject.request_status.code == "REQUEST_FAILED" || responseObject.result.compile_status != "OK")
                    {
                        MessageBox.Show("REQUEST_FAILED");
                        return (null, null);
                    }
                    if (responseObject.request_status.code != "REQUEST_COMPLETED")
                    {
                        MessageBox.Show($"{responseObject}");
                        Thread.Sleep(1500);
                        GetStatus(id);
                    }
                    else
                    {
                        MessageBox.Show($"Returned URI: {responseObject.result.run_status.output}");
                        return (responseObject.result.run_status.status, responseObject.result.run_status.output);
                    }
                }
                return (null, null);
            }
        }
        public void GetStats(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("client-secret", _clientSecret);

                var response = client.GetAsync(_codeEvaluationURL + id).Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;

                dynamic responseObject = JObject.Parse(responseContent);
                MessageBox.Show($"Memory used: {responseObject.result.run_status.memory_used} Kb\n" +
                                $"Time used: {responseObject.result.run_status.time_used} s");
            }
        }
        public string GetOutput(string outputURL)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(outputURL).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                return content;
            }
        }
        public void GetOutputToFIle(string outputURL)
        {
            try
            {
                string fileName = "output.txt";
                string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                using (HttpClient client = new HttpClient())
                {
                    var response = client.GetAsync(outputURL).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    File.WriteAllText(outputPath, content);

                    Console.WriteLine("Successfully saved to file!");
                    Console.WriteLine(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}