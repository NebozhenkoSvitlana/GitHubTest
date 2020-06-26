using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubAnalyzer
{
    public class GitHubResponse
    {
        public string Endpoint { get; set; }

        public string Request { get; set; } 

        public int Count { get; set; }
    }
}
