using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GitHubAnalyzer
{
    public class RequestManager
    {
        private static IHttpRequestMessage authData;

        public static async Task<HttpResponseMessage> GetResponseFromGitHub()
        {
            authData = await ConsoleCustomHandler.GetCustomAuthType();
            return await GetAsync();
        }

        private static async Task<HttpResponseMessage> GetAsync()
        {
            var result  = new HttpResponseMessage(HttpStatusCode.BadRequest);

            using (var client = new HttpClient())
            {
                try
                {
                    result = await client.SendAsync(authData.requestMessage);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return result;
        }
    }
}
