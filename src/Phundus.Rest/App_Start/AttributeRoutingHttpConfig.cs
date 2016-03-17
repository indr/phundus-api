 //[assembly: WebActivator.PreApplicationStartMethod(typeof(Phundus.Rest.AttributeRoutingHttpConfig), "Start")]

namespace Phundus.Rest
{
    using System.Net.Http;
    using System.Reflection;
    using System.Web.Http;
    using AttributeRouting.Web.Http;
    using AttributeRouting.Web.Http.WebHost;

// ReSharper disable InconsistentNaming
    public class PATCHAttribute : HttpRouteAttribute
// ReSharper restore InconsistentNaming
    {
        ///
        /// Specify a route for a GET request.
        ///
        /// The url that is associated with this action
        public PATCHAttribute(string routeUrl) : base(routeUrl, new HttpMethod("PATCH"))
        {
        }
    }

    public static class AttributeRoutingHttpConfig
    {
        public static void RegisterRoutes(HttpRouteCollection routes)
        {
            // See http://github.com/mccalltd/AttributeRouting/wiki for more options.
            // To debug routes locally using the built in ASP.NET development server, go to /routes.axd


            routes.MapHttpAttributeRoutes(a =>
            {
                a.AddRoutesFromAssembly(Assembly.GetExecutingAssembly());
                a.AddRoutesFromAssemblyOf<CoreInstaller>();
            });
        }

        public static void Start()
        {
            RegisterRoutes(GlobalConfiguration.Configuration.Routes);
        }
    }
}