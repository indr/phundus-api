namespace Phundus.Rest.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Runtime.InteropServices;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using ContentObjects;
    using Dashboard.Projections;

    [RoutePrefix("api/event-log")]
    [Authorize(Roles = "Admin")]
    public class EventLogController : ApiControllerBase
    {
        private readonly IEventLogQueries _eventLogQueries;

        public EventLogController(IEventLogQueries eventLogQueries)
        {
            _eventLogQueries = eventLogQueries;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<EventLogData> Get()
        {
            var result = _eventLogQueries.Query();

            return new QueryOkResponseContent<EventLogData>(result);
        }
    }
}