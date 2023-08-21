﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace LeetCodeClone
{
    class LeetCodeApi
    {
        private const string _URI = "https://leetcode.com/api/problems/all/";
        private const string _GraphQLEndpoint = "https://leetcode.com/graphql";
        public async Task<LeetCodeProblemDescription> GetProblemDetailAsync(string titleSlug)
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

                return new LeetCodeProblemDescription
                {
                    Title = jsonResponse.data.question.title,
                    Content = jsonResponse.data.question.content
                };
            }
        }

        public async Task<List<LeetCodeProblem>> GetProblemsAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_URI);
                var responseContent = await response.Content.ReadAsStringAsync();

                dynamic responseObject = JObject.Parse(responseContent);

                List<LeetCodeProblem> problems = new List<LeetCodeProblem>();

                foreach (var problem in responseObject.stat_status_pairs)
                {
                    problems.Add(new LeetCodeProblem
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
}