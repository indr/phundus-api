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
            var command = new ResetProjection(CurrentUserId, projectionId);
            Bus.Send(command);

            return Accepted(command);
        }

        [PATCH("{projectionId}")]
        public virtual HttpResponseMessage Patch(string projectionId)
        {
            var command = new UpdateProjection(CurrentUserId, projectionId);
            Bus.Send(command);

            return Accepted(command);
        }

        [DELETE("{projectionId}")]
        public virtual HttpResponseMessage Delete(string projectionId)
        {
            var command = new RecreateProjection(CurrentUserId, projectionId);
            Bus.Send(command);

            return Accepted(command);
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