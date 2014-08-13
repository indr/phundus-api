namespace Phundus.Rest
{
    using System.Web.Http;
    using Filters;
    using Newtonsoft.Json.Serialization;

    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new AuthorizeAttribute());
            config.Filters.Add(new CustomExceptionFilterAttribute());

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.XmlFormatter.UseXmlSerializer = true;

            AttributeRoutingHttpConfig.RegisterRoutes(config.Routes);
        }
    }
}