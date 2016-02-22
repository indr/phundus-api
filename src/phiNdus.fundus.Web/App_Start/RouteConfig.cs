namespace Phundus.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Index", "",
                new {controller = "Index", action = "Index"});

            routes.MapRoute("Default", "{url}",
                new {controller = "Html5Mode", action = "Index"});
        }
    }
}