namespace phiNdus.fundus.Web.App_Start
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Phundus.Core.Ddd;
    using Phundus.Core.IdentityAndAccess.Organizations.Model;
    using Phundus.Web;

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
                defaults: new {action = "Index", id = UrlParameter.Optional},
                constraints: new {orgId = @"^[\d]+$"}
                );

            routes.MapRoute(
                "Management",
                "management/{id}",
                new {controller = "Management", action = "Index"});

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
        private static readonly IDictionary<string, int> _organizations = new ConcurrentDictionary<string, int>();

        public OrganizationExistsConstraint(IEnumerable<Organization> organizations)
        {
            foreach (var each in organizations)
                _organizations.Add(each.Url, each.Id);
        }

        public static IDictionary<string, int> Organizations
        {
            get { return _organizations; }
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

    public class OrganizationExistsConstraintUpdater : ISubscribeTo<OrganizationEstablished>
    {
        public void Handle(OrganizationEstablished @event)
        {
            var organizations = OrganizationExistsConstraint.Organizations;

            organizations.Remove(@event.Url);
            if (String.IsNullOrWhiteSpace(@event.Url))
                return;
            organizations.Add(@event.Url, @event.OrganizationId);
        }
    }
}