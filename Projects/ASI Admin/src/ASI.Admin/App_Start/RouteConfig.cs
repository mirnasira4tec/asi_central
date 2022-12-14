using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace asi.asicentral.web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Store",
                url: "Store/{controller}/{action}/{id}",
                namespaces: new string[] { "asi.asicentral.web.Controllers.store" },
                defaults: new { controller = "Orders", action = "List", id = UrlParameter.Optional }
             );

            routes.MapRoute(
                name: "sgr",
                url: "sgr/{controller}/{action}/{id}",
                namespaces: new string[] { "asi.asicentral.web.Controllers.sgr" },
                defaults: new { controller = "Company", action = "List", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}