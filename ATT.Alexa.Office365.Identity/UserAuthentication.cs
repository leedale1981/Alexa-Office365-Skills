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
        private string scopes = ConfigurationManager.AppSettings["ida:GraphScopes"];
        private SessionTokenCache tokenCache { get; set; }

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

        public async Task<string> GetUserName()
        {
            try
            {
                GraphServiceClient client = this.GetAuthenticatedClient();
                if (client != null)
                {
                    Microsoft.Graph.User user = await client.Me.Request().GetAsync();
                    return user.UserPrincipalName;
                }
            }
            catch (ServiceException)
            {
                return null;
            }

            return null;
        }

        public async Task<string> GetUserAccessTokenAsync()
        {
            string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            tokenCache = new SessionTokenCache(
                signedInUserID,
                HttpContext.Current.GetOwinContext().Environment["System.Web.HttpContextBase"] as HttpContextBase);
            //var cachedItems = tokenCache.ReadItems(appId); // see what's in the cache

            ConfidentialClientApplication cca = new ConfidentialClientApplication(
                "https://login.microsoftonline.com/common/v2.0",
                HttpContext.Current.Cache["companyAppId"].ToString(),
                redirectUri,
                new ClientCredential(HttpContext.Current.Cache["companySecret"].ToString()),
                tokenCache);

            try
            {
                AuthenticationResult result = await cca.AcquireTokenSilentAsync(scopes.Split(new char[] { ' ' }));
                return result.Token;
            }

            // Unable to retrieve the access token silently.
            catch (MsalSilentTokenAcquisitionException)
            {
                HttpContext.Current.Response.Redirect("~/account/login");
                return null;
            }
        }
    }
}
