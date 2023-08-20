using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace LeetCodeClone
{
    class LeetCodeApi
    {
        private const string _URI = "https://leetcode.com/api/problems/all/";
        private const string _GraphQLEndpoint = "https://leetcode.com/graphql";
        public async Task<string> GetProblemDetailAsync(string titleSlug)
        {
            using (var client = new HttpClient())
            {
                var requestData = new
                {
                    query = @"
                query getQuestionDetail($titleSlug: String!) {
                    question(titleSlug: $titleSlug) {
                        questionId
                        title
                        difficulty
                        likes
                        dislikes
                        isLiked
                        isPaidOnly
                        stats
                        status
                        content
                        topicTags {
                            name
                        }
                        codeSnippets {
                            lang
                            langSlug
                            code
                        }
                        sampleTestCase
                    }
                }
            ",
                    variables = new
                    {
                        titleSlug
                    }
                };

                var requestDataString = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(requestDataString, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_GraphQLEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                dynamic jsonResponse = JObject.Parse(responseContent);

                string htmlStart = @"<!DOCTYPE html><html><head><style>
                                  body {
                                    background-color:#3a3a3a;
                                    color: white;
                                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                                  }
                                </style></head><body>";
                string htmlEnd = "</body></html>";

                return htmlStart + jsonResponse.data.question.content + htmlEnd;
            }
        }

        public async Task<List<Problem>> GetProblemsAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_URI);
                var responseContent = await response.Content.ReadAsStringAsync();

                dynamic responseObject = JObject.Parse(responseContent);

                List<Problem> problems = new List<Problem>();

                foreach (var item in responseObject.stat_status_pairs)
                {
                    problems.Add(new Problem
                    {
                        QuestionTitleSlug = item.stat.question__title_slug,
                        QuestionTitle = item.stat.question__title,
                        DifficultyLevel = item.difficulty.level
                    });
                }
                return problems;
            }
        }
    }
}