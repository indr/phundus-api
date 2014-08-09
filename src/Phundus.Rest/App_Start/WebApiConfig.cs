namespace Phundus.Rest
{
    using System.Web.Http;
    using System.Web.Routing;
    using Filters;
    using Newtonsoft.Json.Serialization;

    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new AuthorizeAttribute());
            config.Filters.Add(new CustomExceptionFilterAttribute());

            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            config.Formatters.JsonFormatter
                  .SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new {action = RouteParameter.Optional},
                constraints: new {controller = @"^[a-z]+$", action = @"^[a-z]*$"});

            config.Routes.MapHttpRoute(
                name: "OrganizationsMembersLocksRoute",
                routeTemplate:"api/organizations/{organizationId}/members/{memberId}/locks/{id}",
                defaults: new { controller = "Locks", id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "OrganizationsApiRoute",
                routeTemplate: "api/organizations/{organizationId}/{controller}/{id}/{action}",
                defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional },
                constraints: new { organizationId = @"^[\d]+$", id = @"^[\d\-a-z]*$" });

            config.Routes.MapHttpRoute(
                name: "OrganizationApiRoute",
                routeTemplate: "api/{organizationId}/{controller}/{id}/{action}",
                defaults: new {id = RouteParameter.Optional, action = RouteParameter.Optional},
                constraints: new {organizationId = @"^[\d]+$", id = @"^[\d\-a-z]*$"});

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new {id = RouteParameter.Optional, action = RouteParameter.Optional},
                constraints: new {id = @"^[\d]*$"});

            config.Formatters.XmlFormatter.UseXmlSerializer = true;
        }
    }
}