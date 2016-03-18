namespace Phundus.Common.Resources
{
    using System.Net.Http;
    using AttributeRouting.Web.Http;

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
}