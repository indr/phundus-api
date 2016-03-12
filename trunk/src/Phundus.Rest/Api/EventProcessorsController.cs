namespace Phundus.Rest.Api
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Core.Internal;
    using Castle.Transactions;
    using Common.Eventing;
    using Common.Projecting.Application;
    using ContentObjects;
    using Dashboard.Projections;
    using Newtonsoft.Json;

    [RoutePrefix("api/event-processors")]
    [Authorize(Roles = "Admin")]
    public class EventProcessorsController : ApiControllerBase
    {
        private readonly IEventProcessorsQueries _eventProcessorsQueries;
        private readonly IEventStore _eventStore;

        public EventProcessorsController(IEventProcessorsQueries eventProcessorsQueries,
            IEventStore eventStore)
        {
            _eventProcessorsQueries = eventProcessorsQueries;
            _eventStore = eventStore;
        }

        [GET("")]
        [Transaction]
        public virtual EventProcessorsQueryOkResponseContent GetAll()
        {
            var maxEventId = _eventStore.GetMaxNotificationId();
            var results = _eventProcessorsQueries.Query();

            return new EventProcessorsQueryOkResponseContent(maxEventId, results);
        }

        [PUT("{processorId}")]
        public virtual HttpResponseMessage Put(string processorId)
        {
            var command = new ResetProjection(CurrentUserId, processorId);
            Bus.Send(command);

            return Accepted(command);
        }

        [PATCH("{processorId}")]
        public virtual HttpResponseMessage Patch(string processorId)
        {
            var command = new UpdateProjection(CurrentUserId, processorId);
            Bus.Send(command);

            return Accepted(command);
        }

        [DELETE("{processorId}")]
        public virtual HttpResponseMessage Delete(string processorId)
        {
            var command = new RecreateProjection(CurrentUserId, processorId);
            Bus.Send(command);

            return Accepted(command);
        }
    }

    public class EventProcessorsQueryOkResponseContent : QueryOkResponseContent<EventProcessorData>
    {
        public EventProcessorsQueryOkResponseContent(long maxEventId,
            IEnumerable<EventProcessorData> results)
            : base(results)
        {
            MaxEventId = maxEventId;
            Results.ForEach(metaData => metaData.SetBehind(MaxEventId));
        }

        [JsonProperty("MaxEventId")]
        public long MaxEventId { get; set; }
    }
}