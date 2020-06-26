using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GitHubAnalyzer.AuthTypes
{
    public class OAuthManager
    {
        private static string RefreshTokenUrl = "https://github.com/NebozhenkoSvitlana";
        private static string GithubClientId = "de7ff1430a553f626701";
        private static string GithubClientSecret = "17c94a8e62fc17f51b55451ba3b5addc9c6da17b";

        public static async Task<string> GetAcessTokenForUser() 
        {
            var resultToken = await GetNewAccessTokenAsync(RefreshTokenUrl, GithubClientId, GithubClientSecret);
            return resultToken.Token;
        }

        private static async Task<AccessTokenResult> GetNewAccessTokenAsync(string refreshToken, string githubClientId, string githubClientSecret)
        {
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            handler.UseCookies = true;

            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["refresh_token"] = refreshToken,
                ["client_id"] = githubClientId,
                ["client_secret"] = githubClientSecret,
                ["grant_type"] = "refresh_token"
            });


            using (var client = new HttpClient(handler))
            {
                //client.DefaultRequestHeaders.Add("user-agent", "SvitlanaTest");
                //client.DefaultRequestHeaders.Add("Accept", "application/json");
                
                var result = new HttpResponseMessage(HttpStatusCode.BadRequest);
                try
                {
                    result = await client.PostAsync(RefreshTokenUrl, content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var respContent = await result.Content.ReadAsStringAsync();
                    var tokenInfo = JsonConvert.DeserializeObject<GitHubRefreshAccessTokenResponse>(respContent);
                    int.TryParse(tokenInfo.expires_in, out var expiresInSeconds);
                    if (expiresInSeconds <= 0)
                    {
                        expiresInSeconds = 180;
                    }
                    return AccessTokenResult.RefreshedResult(tokenInfo.access_token, TimeSpan.FromSeconds(expiresInSeconds));
                }
                else
                {
                    var respContent = await result.Content.ReadAsStringAsync();
                    return AccessTokenResult.ErrorResult(new GitHubConnectionException(new Exception(respContent)));
                }
            }
        }
        public class AccessTokenResult
        {
            public string Token { get; }
            public bool IsSuccess { get; }
            public Exception Error { get; }
            public bool IsRefreshed { get; }
            public TimeSpan? ExpiresIn { get; }

            public static AccessTokenResult RefreshedResult(string token, TimeSpan expiresIn)
            {
                return new AccessTokenResult(token, true, true, expiresIn, null);
            }

            public static AccessTokenResult ErrorResult(Exception error)
            {
                return new AccessTokenResult(null, false, false, null, error);
            }

            private AccessTokenResult(string token, bool isSuccess, bool isRefreshed, TimeSpan? expiresIn, Exception error)
            {
                Token = token;
                IsSuccess = isSuccess;
                IsRefreshed = isRefreshed;
                ExpiresIn = expiresIn;
                Error = error;
            }
        }

        [Serializable]
        public class GitHubRefreshAccessTokenResponse
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
        }

        public class GitHubConnectionException : Exception
        {
            public GitHubConnectionException(Exception inner)
                : base("Connection to GitHub API failed.", inner)
            {
            }
        }
    }
}
