using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATT.Alexa.Office365.Service.Models
{
    public class AppBuilderProvider : IDisposable
    {
        private IAppBuilder _app;
        public AppBuilderProvider(IAppBuilder app)
        {
            _app = app;
        }
        public IAppBuilder Get() { return _app; }
        public void Dispose() { }
    }
}