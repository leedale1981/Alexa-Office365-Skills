using ATT.Alexa.Office365.Identity;
using ATT.Alexa.Office365.Models;
using ATT.Alexa.Office365.Repositories;
using ATT.Alexa.Office365.Service.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ATT.Alexa.Office365.Service.Controllers
{
    public class AccountController : Controller
    {
        private ICreateRepositoryAsync<User> createUserRepository = null;
        private IReadRepositoryAsync<User> readUserRepository = null;
        private IReadRepositoryAsync<Company> readCompanyRepository = null;
        private const string StateSessionKey = "alexastate";
        private const string ScopeSessionKey = "alexascope";
        private const string RedirectUriSessionKey = "alexaredirecturi";
        private const string TokenTypeSessionKey = "alexatokentype";

        public AccountController(
            ICreateRepositoryAsync<User> createUserRepository, 
            IReadRepositoryAsync<User> readUserRepository,
            IReadRepositoryAsync<Company> readCompanyRepository)
        {
            this.createUserRepository = createUserRepository;
            this.readUserRepository = readUserRepository;
            this.readCompanyRepository = readCompanyRepository;
        }
        
        public async Task<ActionResult> Login(
            string state, 
            string client_id, 
            string response_type, 
            string scope, 
            string redirect_uri,
            string token_type)
        {
            this.Session[StateSessionKey] = state;
            this.Session[ScopeSessionKey] = scope;
            this.Session[RedirectUriSessionKey] = redirect_uri;
            this.Session[TokenTypeSessionKey] = token_type;

            var companies = await ((CompanyRepository)this.readCompanyRepository).ReadAll();

            if (companies != null)
            {
                var selectListItems = companies.Select(c => new SelectListItem() { Text = c.CompanyName, Value = c.Id });

                CompanyViewModel viewModel = new CompanyViewModel()
                {
                    Companies = selectListItems
                };

                return View(viewModel);
            }

            return new HttpStatusCodeResult(500);
        }


        public void Authenticate(CompanyViewModel viewModel)
        {
            Company company = new Company() { Id = viewModel.SelectedCompanyId };
            company = Task.Run<Company>(async () => { return await this.readCompanyRepository.Read(company); }).Result;

            // Store company registration details;
            HttpContext.Cache["companyAppId"] = company.AppId;
            HttpContext.Cache["companySecret"] = company.AppSecret;
            HttpContext.Cache["companyId"] = company.Id;

            // Autheticate the user against selected company;
            HttpContext.GetOwinContext().Authentication.Challenge(
                new AuthenticationProperties { RedirectUri = "/account/linkuser" },
                OpenIdConnectAuthenticationDefaults.AuthenticationType);
        }

        public async Task<ActionResult> LinkUser()
        {
            User user = new User();
            user.CompanyId = HttpContext.Cache["companyId"].ToString();
            string signedInUserID = ((ClaimsPrincipal)Thread.CurrentPrincipal).FindFirst(ClaimTypes.NameIdentifier).Value;

            // Authenticate user with O365
            UserAuthentication auth = new UserAuthentication(user);
            string upn = await auth.GetUserName();

            if (!string.IsNullOrEmpty(upn))
            {
                // Create user in DB
                user.Id = signedInUserID;
                user.UserName = upn;

                string appId = HttpContext.Cache["companyAppId"].ToString();
                var cachedItems = new SessionTokenCache(signedInUserID, HttpContext).ReadItems(appId);
                user.AccessToken = cachedItems.ToList()[0].Token;
                await createUserRepository.Create(user);

                // Redirect to Alexa app with user id.
                string queryStrings = "&state=" + HttpUtility.HtmlEncode(this.Session[StateSessionKey])
                    + "&access_token=" + HttpUtility.HtmlEncode(user.Id)
                    + "&token_type=Bearer";
                this.Response.Redirect(this.Session[RedirectUriSessionKey] + queryStrings);
                return null;
            }
            else
            {
                return new RedirectResult("/account/login");
            }
        }
    }
}