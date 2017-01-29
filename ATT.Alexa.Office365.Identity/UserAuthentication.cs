using ATT.Alexa.Office365.Models;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Security.Claims;
using System.Net.Http.Headers;

namespace ATT.Alexa.Office365.Identity
{
    public class UserAuthentication
    {
        private Models.User user = null;
        private string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private string appId = ConfigurationManager.AppSettings["ida:AppId"];
        private string appSecret = ConfigurationManager.AppSettings["ida:AppSecret"];
        private string scopes = ConfigurationManager.AppSettings["ida:GraphScopes"];

        public UserAuthentication(Models.User user)
        {
            this.user = user;
        }

        public GraphServiceClient GetAuthenticatedClient()
        {
            GraphServiceClient client = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                        async (requestMessage) =>
                        {
                            string accessToken = await this.GetUserAccessTokenAsync();

                            // Append the access token to the request.
                            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                        }
                    ));

            return client;
        }

        public bool IsAuthenticated()
        {
            try
            {
                GraphServiceClient client = this.GetAuthenticatedClient();
                if (client != null)
                {
                    return true;
                }
            }
            catch (ServiceException)
            {
                return false;
            }

            return false;
        }

        private async Task<string> GetUserAccessTokenAsync()
        {
            SessionTokenCache tokenCache = new SessionTokenCache(
                this.user.UserId,
                HttpContext.Current.GetOwinContext().Environment["System.Web.HttpContextBase"] as HttpContextBase);

            ConfidentialClientApplication cca = new ConfidentialClientApplication(
                appId,
                redirectUri,
                new ClientCredential(appSecret),
                tokenCache);

            try
            {
                AuthenticationResult result = await cca.AcquireTokenSilentAsync(scopes.Split(new char[] { ' ' }));
                return result.Token;
            }

            // Unable to retrieve the access token silently.
            catch (MsalSilentTokenAcquisitionException)
            {
                HttpContext.Current.Request.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties() { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);

                throw new ServiceException(
                    new Error
                    {
                        Code = GraphErrorCode.AuthenticationFailure.ToString(),
                        Message = "Authentication Failed",
                    });
            }
        }
    }
}
