namespace Phundus.Rest.Api
{
    using System;
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

    [RoutePrefix("api/notification-processors")]
    [Authorize(Roles = "Admin")]
    public class NotificationProcessorsController : ApiControllerBase
    {
        private readonly IEventStore _eventStore;
        private readonly IProcessedNotificationQueries _processedNotificationQueries;

        public NotificationProcessorsController(IProcessedNotificationQueries processedNotificationQueries, IEventStore eventStore)
        {
            _processedNotificationQueries = processedNotificationQueries;
            _eventStore = eventStore;
        }

        [GET("")]
        [Transaction]
        public virtual NotificationProcessorsQueryOkResponseContent GetAll()
        {
            var maxEventId = _eventStore.GetMaxNotificationId();
            var results = _processedNotificationQueries.Query();

            return new NotificationProcessorsQueryOkResponseContent(maxEventId, results);
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

    public class NotificationProcessorsQueryOkResponseContent : QueryOkResponseContent<ProcessedNotificationData>
    {
        public NotificationProcessorsQueryOkResponseContent(long maxEventId, IEnumerable<ProcessedNotificationData> results)
            : base(results)
        {
            MaxEventId = maxEventId;
            Results.ForEach(metaData => metaData.SetBehind(MaxEventId));
        }

        [JsonProperty("MaxEventId")]
        public long MaxEventId { get; set; }
    }
}