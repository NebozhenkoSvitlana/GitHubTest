using System;
using System.Threading.Tasks;

namespace GitHubAnalyzer
{
    public class ResultBuilder
    {
        public static async Task BuildAPIAnalysisResult()
        {
            ResponseAnalyzer analyzer = new ResponseAnalyzer();
            var allEndpoints = await analyzer.GetAllGitHubEndpoints();
            var categoryCounts = analyzer.GetEndpointsCategory(allEndpoints);
            var queryEndpoints = analyzer.GetEndpointsWithQuery(allEndpoints);

            Console.WriteLine("Availabled endpoints: " + analyzer.GetGitHubEndpointsCount(allEndpoints));
            Console.WriteLine("***********************************************************************");

            Console.WriteLine("GitHub API requests categories: ");
            Console.WriteLine("--------------------------------");
            foreach (var item in categoryCounts)
            {
                Console.WriteLine(item.Key + ": " + item.Value + ", ");
            }
            Console.WriteLine("***********************************************************************");
            Console.WriteLine("GitHub API requests with query: ");
            Console.WriteLine("--------------------------------");

            foreach (var item in queryEndpoints)
            {
                Console.WriteLine(item.Key);
            }
            Console.WriteLine("***********************************************************************");
            Console.ReadKey();
        }
    }
}
