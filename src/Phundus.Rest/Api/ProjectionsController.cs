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

    [RoutePrefix("api/projections")]
    [Authorize(Roles = "Admin")]
    public class ProjectionsController : ApiControllerBase
    {
        private readonly IEventStore _eventStore;
        private readonly IProjectionsMetaDataQueries _projectionsMetaDataQueries;

        public ProjectionsController(IProjectionsMetaDataQueries projectionsMetaDataQueries, IEventStore eventStore)
        {
            if (projectionsMetaDataQueries == null) throw new ArgumentNullException("projectionsMetaDataQueries");
            if (eventStore == null) throw new ArgumentNullException("eventStore");

            _projectionsMetaDataQueries = projectionsMetaDataQueries;
            _eventStore = eventStore;
        }

        [GET("")]
        [Transaction]
        public virtual ProjectionsQueryOkResponseContent GetAll()
        {
            var maxEventId = _eventStore.GetMaxNotificationId();
            var results = _projectionsMetaDataQueries.Query();
            return new ProjectionsQueryOkResponseContent(maxEventId, results);
        }

        [PUT("{projectionId}")]
        public virtual HttpResponseMessage Put(string projectionId)
        {
            Bus.Send(new ResetProjection(CurrentUserId, projectionId));

            return Accepted();
        }

        [PATCH("{projectionId}")]
        public virtual HttpResponseMessage Patch(string projectionId)
        {
            Bus.Send(new UpdateProjection(CurrentUserId, projectionId));

            return Accepted();
        }

        [DELETE("{projectionId}")]
        public virtual HttpResponseMessage Delete(string projectionId)
        {
            Bus.Send(new RecreateProjection(CurrentUserId, projectionId));

            return Accepted();
        }
    }

    public class ProjectionsQueryOkResponseContent : QueryOkResponseContent<ProjectionMetaData>
    {
        public ProjectionsQueryOkResponseContent(long maxEventId, IEnumerable<ProjectionMetaData> results)
            : base(results)
        {
            MaxEventId = maxEventId;
            Results.ForEach(metaData => metaData.SetBehind(MaxEventId));
        }

        [JsonProperty("MaxEventId")]
        public long MaxEventId { get; set; }
    }
}