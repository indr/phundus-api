namespace Phundus.Rest
{
    using System.Web.Http;
    using Newtonsoft.Json.Serialization;

    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            config.Formatters.JsonFormatter
                  .SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new {action = RouteParameter.Optional},
                constraints: new {controller = @"^[a-z]+$", action = @"^[a-z]*$"});

            config.Routes.MapHttpRoute(
                name: "OrganizationApiRoute",
                routeTemplate: "api/{organization}/{controller}/{id}/{action}",
                defaults: new {id = RouteParameter.Optional, action = RouteParameter.Optional},
                constraints: new {organization = @"^[\d]+$", id = @"^[\d\-a-z]*$"});

            //config.Routes.MapHttpRoute(
            //    name: "OrganizationsApiRoute",
            //    routeTemplate: "api/organizations/{orgId}/{controller}/{action}/{id}",
            //    defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional },
            //    constraints: new { orgId = @"^[\d]+$" });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new {id = RouteParameter.Optional, action = RouteParameter.Optional},
                constraints: new {id = @"^[\d]*$"});

            config.Formatters.XmlFormatter.UseXmlSerializer = true;
        }
    }
}