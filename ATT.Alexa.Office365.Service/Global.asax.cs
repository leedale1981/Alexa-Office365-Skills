using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Optimization;
using Microsoft.IdentityModel.Protocols;

namespace ATT.Alexa.Office365.Service
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Session_Start(object sender, EventArgs e)
        {
        }

        void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Handle nonce exception
            var ex = Server.GetLastError();
            if (ex.GetType() == typeof(OpenIdConnectProtocolInvalidNonceException) & HttpContext.Current.IsCustomErrorEnabled)
            {
                Server.ClearError();
                Response.Redirect("~/account/login");
            }

        }
    }
}