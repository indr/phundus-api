namespace phiNdus.fundus.Web.App_Start
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Phundus.Core.IdentityAndAccess.Organizations.Model;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes, IEnumerable<Organization> organizations)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Organization",
                "{name}",
                new {controller = ControllerNames.Organization, action = "Home"},
                new {name = new OrganizationExistsConstraint(organizations)});

            routes.MapRoute(
                "Orgs",
                "orgs/{orgId}/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional },
                constraints: new { orgId = @"^[\d]+$" }
        );

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
        readonly IDictionary<string, int> _organizations = new Dictionary<string, int>();

        public OrganizationExistsConstraint(IEnumerable<Organization> organizations)
        {
            foreach (var each in organizations)
                _organizations.Add(each.Url, each.Id);
        }

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