using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace Frontend.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static Container Container { get; set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Container = Business.Services.Configurations.SimpleInjectorInitializer.ApiInitialize();
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(Container);

            //Business.Services.Configurations.AutoMapperConfiguration.Configure();
        }
    }
}
