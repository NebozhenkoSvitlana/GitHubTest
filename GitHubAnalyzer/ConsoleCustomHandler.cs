using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubAnalyzer
{
    public class ConsoleCustomHandler
    {
        private static string user;
        private static string customType = "1";
        private static int isErrorState = 0;
        private static IHttpRequestMessage authData;

        public static async Task<IHttpRequestMessage> GetCustomAuthType() 
        {
            List<string> authenticationTypes = new List<string> { "1", "2", "3" };
            bool isAuthTypeTrue = false;

            Console.WriteLine("Please, enter the type of authentication (1 - Without, 2 - Basic, 3 - QAuth): ");
            while (!isAuthTypeTrue)
            {
                customType = Console.ReadLine();
                if (!authenticationTypes.Contains(customType))
                {
                    Console.WriteLine("Your choose is incorrect. Please, type valid (1 - Without, 2 - Basic, 3 - QAuth): ");
                }
                else
                {
                    isAuthTypeTrue = true;
                }
            }

            switch (customType)
            {
                case "1":
                    authData = new HttpRequestMessageWithoutAuth();
                    break;
                case "2":
                    authData = new HttpRequestMessageWithBasicAuth(GetUserNameForAuth());
                    break;
                case "3":
                    var token = await AuthTypes.OAuthManager.GetAcessTokenForUser();
                    authData = new HttpRequestMessageWithQAuth2(token);
                    break;
            }

            return authData;
        }

        public static string GetUserNameForAuth()
        {
            if (isErrorState < 5 || isErrorState >= 0) 
            {
                Console.WriteLine("Please, enter correct UserName for basic authentication: ");
                user = Console.ReadLine();
                isErrorState = isErrorState + 1;
            }
            if(isErrorState >= 5)
            {
                Console.WriteLine("You expired your attempts. Please, try later. Good bye!");
                Environment.Exit(0);
            }

            return user;
        }
    }
}
