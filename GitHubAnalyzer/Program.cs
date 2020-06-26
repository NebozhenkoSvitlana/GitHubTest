using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubAnalyzer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await ResultBuilder.BuildAPIAnalysisResult();
        }
    }
}
