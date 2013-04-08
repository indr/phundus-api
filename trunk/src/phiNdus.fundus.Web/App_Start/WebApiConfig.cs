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
               name: "DefaultApi",
               routeTemplate: "api/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional },
               constraints: new { id = @"^[\d]*$" }
               );

            config.Routes.MapHttpRoute(
                name: "OrganizationApiRoute",
                routeTemplate: "api/{organization}/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                
                );

           
        }
    }
}