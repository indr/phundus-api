namespace phiNdus.fundus.Web.App_Start
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
                constraints: new {organization = @"^[\d]+$", id = @"^[\d]*$"});

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new {id = RouteParameter.Optional, action = RouteParameter.Optional},
                constraints: new {id = @"^[\d]*$"});
        }
    }
}