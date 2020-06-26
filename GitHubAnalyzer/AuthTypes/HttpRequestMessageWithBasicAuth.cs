using System;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;

namespace GitHubAnalyzer
{
    public class HttpRequestMessageWithBasicAuth: HttpRequestMessageWithoutAuth
    {
        public HttpRequestMessageWithBasicAuth(string user) 
        {
            requestMessage.Headers.Add("Authorization", Convert.ToBase64String(Encoding.UTF8.GetBytes(user)));
        }
    }
}
