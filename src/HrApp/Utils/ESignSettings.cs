using DocuSign.eSign.Client;
using DocuSign.eSign.Client.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DocuSign.eSign.Client.Auth.OAuth.UserInfo;

namespace HrApp
{
    public class ESignSettings
    {
        private static string ClientID = "3e126cce-1a34-44fe-b87a-534cd94db250";
        private static string ImpersonatedUserGuid = "afc05117-8cdc-479f-83fe-9d3db4a3db4e";
        private static string PrivateKey = "MIIEpAIBAAKCAQEAhY1nsFTNaVNj3xfqWDs3Xf6KWgeGfRszVh/cQ6AqZvDQs6q5tqu0WwQqSInShlIavX1BlX0GGLdYEMSCHozLfMD7DhgkMCXJWNkRyCdYKV9LCBhy9xNDWRJ/g6CALIPe11RBRoTcGaQt7euYVe+1LhEXN+thls8gtg3r02X5C8izTYEPYl1DKhxBai/NDJ17Emvu7oPGeYXfhgSZlW64KAMTwTVIm1oi87pPJtRa7aoj7DF5QoG4vv5D3SEi3qTxcPqOPNSjmoQivP7gRD1LpnVYbOgw2aphwU345GpCDfcHEj1G6HxjXWhezNNxZqWhmDziCaGfH2fvNIGMAeZEawIDAQABAoIBAA1SfUDsPK9gh6pl/tT9VzkTnqZ7cLGDKJxTQkwmqoQpSpBZm/v6ZdvcbwFcajlG8G7tk0C/v39wrqLBj39PqS7RK1oNE3MQGeU4hhsSvasm29u7YKB/9expOXoeZRrLrsuJFpfbwf72zzQYF9KevlKRluDXpJ0AHXG7ssYcOnsvzbsVR4MIvqs72q6KmErzKzFTSkzE2DINkWaLFe2G+kKr5XzOA+MTzgNqhQUyanQAL1QJss5jW0urarCqmuKp3M027ePG/10l7PXx7gjf7f2LQ2XlXyYxmM4GwhDkDUY+1mciUxl01hPEyopQ0D7TNpC6GD1b1wtnVp8+JibmAi0CgYEAugV6jk8Vk0ysYmESBY+wB1g06HcDByKyzLQFbC+HrkyOQnQYtsWExrsUsCLCOyvfDkMVIL5iWCceau++BfWUffMdONj9vDr+CpGTpPqzodnE8EM+ym4YcGudeqbWw9HneujqEpyBT01NnoQRjx6/oXoHSxtYQhA/YWxGVFHk/3cCgYEAt8r4wXqmITrycwA3akTakm2VAFELHbc0DUZlhQzmAlU71d/56B4r78M6cj/AS8tSsqiexRmLuOUlUgW2dc9JwuIGA+6LQ2Pv+auH5YSNIlTBcXTqB51KwJPK6/ATkyI7Lx8YfwotVlW724PQfNq5wWNbehedjvodfZE46Uo5p60CgYEAovqn3MUHRy33Hp6jloHEnTq36DBVU8wf0V+sHJQsERb70Nc8y+2UHjXMs2FQwVz3qdw2R6DmMwvbB6AS4c+/EPaO1L6w2FjrOMFBFVudRKGyfTq9acg5200+BTbllV1zrOkiI7pBRQSai9Z0N+udw9FRUImdswvfK/EcUUYLF5MCgYAmL1VfXUwGu2iD3BenTIbcxUefuTbGBboax/Vvny5qbarw1IfnYd1fAwCxm+0n0iZsV/wklxogX/tQ/z7ZWyfIBY2aY4Uriyfgh3LEjLWF3HYUTGYTDuro98vBGS/38bS6JYvBWSvyM/3Dra8zQX42X54xz5Y8jlpyLCnWzkUMQQKBgQCoG884ak+LGczZ0u2fMnV98B8a0Ab9pO8B1ZHEy/t+XM3d5gVlgujPrGnAntyn5KpJ/sBaOsD3/dVbpFIV4IXCcIWACwRJYMBzDwO9Ki9nAtnOSe5hDWWw/GUG6MfVc3itsww8GMsQKIGZCgjNt3lfwZrGlUAsfphXJSczakpoNw==";
    private static string TargetAccountID = "969c6cd3-c1bc-495f-8268-3aabc0c38888";
    private static string AuthServer = "account-d.docusign.com";

    // These properties must be defined for the example code to work correctly, but you do not need to edit them.
    private const int TOKEN_REPLACEMENT_IN_SECONDS = 10 * 60;

    private static string AccessToken { get; set; }
    private static int expiresIn;
    private static Account Account { get; set; }

    protected static ApiClient ApiClient { get; private set; }

    protected static string AccountID
    {
        get { return Account.AccountId; }
    }

        public void CheckToken()
        {
            if (AccessToken == null
                || (DateTime.Now.Millisecond + TOKEN_REPLACEMENT_IN_SECONDS) > expiresIn)
            {
                Console.WriteLine("Obtaining a new access token...");
                UpdateToken();
                // This method will be implemented in the next step
            }
        }

        private void UpdateToken()
        {
            OAuth.OAuthToken authToken = ApiClient.RequestJWTUserToken(ClientID,
                            ImpersonatedUserGuid,
                            AuthServer,
                            Encoding.UTF8.GetBytes(PrivateKey),
                            1);

            AccessToken = authToken.access_token;

            if (Account == null)
                Account = GetAccountInfo(authToken);

            ApiClient = new ApiClient(Account.BaseUri + "/restapi");

            expiresIn = DateTime.Now.Second + authToken.expires_in.Value;
        }

        private Account GetAccountInfo(OAuth.OAuthToken authToken)
        {
            ApiClient.SetOAuthBasePath(AuthServer);
            OAuth.UserInfo userInfo = ApiClient.GetUserInfo(authToken.access_token);
            Account acct = null;

            var accounts = userInfo.Accounts;

            if (!string.IsNullOrEmpty(TargetAccountID) && !TargetAccountID.Equals("FALSE"))
            {
                acct = accounts.FirstOrDefault(a => a.AccountId == TargetAccountID);

                if (acct == null)
                {
                    throw new Exception("The user does not have access to account " + TargetAccountID);
                }
            }
            else
            {
                acct = accounts.FirstOrDefault(a => a.IsDefault == "true");
            }

            return acct;
        }
    }
}
