using System;
using System.Text;
using System.Net.Http;

namespace GitHubAnalyzer
{
    public class HttpRequestMessageWithoutAuth: IHttpRequestMessage
    {
        private static string requestResource = "https://api.github.com/";
        private static string userAgent = "SvitlanaTest";
        public HttpRequestMessage requestMessage { get; set; }

        public HttpRequestMessageWithoutAuth() 
        {
            requestMessage = BuildBody();
        }

        public HttpRequestMessage BuildBody() 
        {
            var message = new HttpRequestMessage(HttpMethod.Get, requestResource);
            message.Headers.TryAddWithoutValidation("user-agent", userAgent);
            message.Content = new StringContent("application/json");
            return message;         
        }
    }
}
