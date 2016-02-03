namespace Phundus.Rest.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Dashboard.Queries;
    using Dashboard.Querying;

    [RoutePrefix("api/event-log")]
    [Authorize(Roles = "Admin")]
    public class EventLogController : ApiControllerBase
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