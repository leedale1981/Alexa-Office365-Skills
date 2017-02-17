using ATT.Alexa.Office365.Service.Models;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ATT.Alexa.Office365.Service.Startup))]
namespace ATT.Alexa.Office365.Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
