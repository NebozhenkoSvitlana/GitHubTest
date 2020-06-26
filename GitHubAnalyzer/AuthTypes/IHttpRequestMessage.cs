using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace GitHubAnalyzer
{
    public interface IHttpRequestMessage
    {
        public HttpRequestMessage requestMessage { get; set; }
        HttpRequestMessage BuildBody();
    }
}
