using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace LeetCodeClone
{
    public static class LeetCode
    {
        private const string _URI = "https://leetcode.com/api/problems/all/";
        private const string _GraphQLEndpoint = "https://leetcode.com/graphql";
        private static readonly HttpClient _httpClient;

        static LeetCode()
        {
            _httpClient = new HttpClient();
        }

        public static async Task<ProblemDescription> GetProblemDetailAsync(string titleSlug)
        {
            var requestData = new
            {
                query = @"
                        query getQuestionDetail($titleSlug: String!) {
                            question(titleSlug: $titleSlug) {
                                title
                                content
                            }
                        }",
                variables = new
                {
                    titleSlug
                }
            };

            var requestDataString = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(requestDataString, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_GraphQLEndpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            dynamic jsonResponse = JObject.Parse(responseContent);

            return new ProblemDescription
            {
                Title = jsonResponse.data.question.title,
                Content = jsonResponse.data.question.content
            };
        }

        public static async Task<List<Problem>> GetProblemsAsync()
        {
            var response = await _httpClient.GetAsync(_URI);
            var responseContent = await response.Content.ReadAsStringAsync();

            dynamic responseObject = JObject.Parse(responseContent);

            List<Problem> problems = new List<Problem>();

            foreach (var problem in responseObject.stat_status_pairs)
            {
                bool isPaidOnly = problem.paid_only;

                if (isPaidOnly)
                {
                    continue;
                }
                problems.Add(new Problem
                {
                    QuestionTitleSlug = problem.stat.question__title_slug,
                    QuestionTitle = problem.stat.question__title,
                    DifficultyLevel = problem.difficulty.level
                });
            }
            return problems;
        }
    }
}