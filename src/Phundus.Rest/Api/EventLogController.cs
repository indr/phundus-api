namespace Phundus.Rest.Api
{
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
        private readonly IEventLogQueryService _eventLogQueryService;

        public EventLogController(IEventLogQueryService eventLogQueryService)
        {
            _eventLogQueryService = eventLogQueryService;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<EventLogData> Get()
        {
            var result = _eventLogQueryService.Query();

            return new QueryOkResponseContent<EventLogData>(result);
        }
    }
}