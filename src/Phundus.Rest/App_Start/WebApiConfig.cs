namespace Phundus.Rest
{
    using System.Web.Http;
    using Converters;
    using Filters;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new AuthorizeAttribute());
            config.Filters.Add(new ExceptionFilterAttribute());

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new CustomDateTimeConverter());
            config.Formatters.XmlFormatter.UseXmlSerializer = true;            

            AttributeRoutingHttpConfig.RegisterRoutes(config.Routes);
        }
    }
}