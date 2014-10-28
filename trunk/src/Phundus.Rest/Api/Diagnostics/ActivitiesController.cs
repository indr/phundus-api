namespace Phundus.Rest.Api.Diagnostics
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.Dashboard.Application;

    [RoutePrefix("api/diagnostics/activities")]
    [Authorize(Roles = "Admin")]
    public class ActivitiesController : ApiControllerBase
    {
        public IActivityQueryService ActivityQueryService { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, ActivityQueryService.FindMostRecent20());
        }
    }
}