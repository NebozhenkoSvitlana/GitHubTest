using GitHubAnalyzer.AuthTypes;
using System.Threading.Tasks;

namespace GitHubAnalyzer
{
    public class HttpRequestMessageWithQAuth2: HttpRequestMessageWithoutAuth
    {   
        public HttpRequestMessageWithQAuth2(string token) 
        {
            requestMessage.Headers.Add("Authorization", "Bearer " + token);
        }
    }
}
