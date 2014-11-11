
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using asi.asicentral.views;

namespace asi.asicentral.web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
			HostingEnvironment.RegisterVirtualPathProvider(new CustomVirtualPathProvider());
			AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
    }
}