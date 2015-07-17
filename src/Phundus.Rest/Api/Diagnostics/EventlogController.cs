namespace Phundus.Rest.Api.Diagnostics
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.Dashboard.Querying;

    [RoutePrefix("api/diagnostics/eventlog")]
    [Authorize(Roles = "Admin")]
    public class EventlogController : ApiControllerBase
    {
        public IEventLogQueries EventLogQueries { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, EventLogQueries.FindMostRecent20());
        }
    }
}