using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace ATT.Alexa.Office365.Service
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/officefabric").Include(
                    "~/Content/fabric.css",
                    "~/Content/fabric.components.css",
                    "~/Content/Office.Controls.AppChrome.min.css"
                    ));

            bundles.Add(new StyleBundle("~/Content/app").Include(
                    "~/Content/app.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/officefabricjs").Include(
                    "~/Scripts/fabric.js",
                    "~/Scripts/Jquery.Dropdown.js"
                ));
        }
    }
}