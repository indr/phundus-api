using System.Web.Mvc;
using System.Web.Routing;

namespace phiNdus.fundus.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = ControllerNames.Home, action = "Index", id = UrlParameter.Optional }
                // Parameter defaults
                );

            routes.MapRoute("ImageStore",
                            "{controller}/{action}/{id}/{name}");
        }
    }
}