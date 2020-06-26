using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections;

namespace GitHubAnalyzer
{
    public class ResponseAnalyzer
    {
        private static List<GitHubResponse> endpoints;

        public ResponseAnalyzer() 
        {
            endpoints = new List<GitHubResponse>();
        }

        private async Task<JObject> GetDataFromGitHubAPI() 
        {
            var result = await RequestManager.GetResponseFromGitHub();

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("You are Unauthorized!");
                return null;
            }
            
            return JObject.Parse(result.Content.ReadAsStringAsync().Result);
        }

        private async Task GetEndpoints()
        {
            var gitHubResponse = await GetDataFromGitHubAPI();

            foreach (JToken token in gitHubResponse.Children())
            {
                if (token is JProperty)
                {
                    var prop = token as JProperty;

                    endpoints.Add(new GitHubResponse { Endpoint = prop.Name, Request = prop.Value.ToString(), Count = 1 });
                }
            }
        }

        public async Task<List<GitHubResponse>> GetAllGitHubEndpoints() 
        {
            try
            {
                await GetEndpoints();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);///need to implement more retries
            }

            return endpoints;
        }

        public Dictionary<string, string> GetEndpointsCategory(List<GitHubResponse> allEndpoints) 
        {

            return allEndpoints.Select(x => new { Key = x.Request.Split(new char[] { '?', '{', '/' }), Value = x.Count })
                                .GroupBy(g => g.Key[3])
                                .Select(r => new { Category = r.Key, Count = r.Sum(y => y.Value) })
                                .OrderByDescending(o => o.Count)
                                .ToDictionary(d => d.Category.ToString(), d => d.Count.ToString(), StringComparer.OrdinalIgnoreCase);
        }

        public Dictionary<string, string> GetEndpointsWithQuery(List<GitHubResponse> allEndpoints) 
        {
            return allEndpoints.Select(x => new { Flag = x.Request.Contains("query"), Request = x.Request })
                               .Where(t => t.Flag == true)
                               .ToDictionary(d => d.Request.ToString(), d => d.Flag.ToString(), StringComparer.OrdinalIgnoreCase);
        }

        public int GetGitHubEndpointsCount(List<GitHubResponse> allEndpoints) 
        {
            return allEndpoints.Count;
        }

    }
}
