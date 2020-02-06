using BusinessServices;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using WebAPIDays.Filters;
using WebAPIDays.ActionFilters;

namespace WebAPIDays
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new LoggingFilterAttribute());
            // Web API configuration and services
            var container = new UnityContainer();
            container.RegisterType<IProductServices, ProductServices>()
                .RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserService,UserServices>()
                .RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            GlobalConfiguration.Configuration.Filters.Add(new ApiAuthenticationFilter());
        }
    }
}
