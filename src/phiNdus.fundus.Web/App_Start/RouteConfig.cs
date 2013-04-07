namespace phiNdus.fundus.Web.App_Start
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Organization",
                "{name}",
                new {controller = ControllerNames.Organization, action = "Home"},
                new {name = new OrganizationExistsConstraint()});

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = ControllerNames.Home, action = "Index", id = UrlParameter.Optional});

            routes.MapRoute(
                "ImageStore",
                "{controller}/{action}/{id}/{name}");
        }
    }

    public class OrganizationExistsConstraint : IRouteConstraint
    {
        // TODO: Verzeichnis anhand Daten in der Datenbank erzeugen.
        readonly IDictionary<string, int> _organizations = new Dictionary<string, int>
            {
                // Acceptance
                {"pfadi-lego", 1001},
                {"jubla-playmobil", 1002},
                {"cevi-dupplo", 1003},

                // Production
                {"pfadi-luzern", 1001},
                {"jasol", 1002},
                {"jubla-luzern", 1003},
                {"sac-pilatus", 1004},
                {"cevi-zürich", 1005},
                {"cevi-zuerich", 1005}
            };

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            var name = values[parameterName].ToString().ToLowerInvariant();

            int id;
            if (!_organizations.TryGetValue(name, out id))
                return false;


            values["id"] = id;
            return true;
        }
    }
}