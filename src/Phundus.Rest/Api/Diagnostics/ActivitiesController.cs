namespace Phundus.Rest.Api.Diagnostics
{
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.Dashboard.Querying;

    [RoutePrefix("api/diagnostics/activities")]
    [Authorize(Roles = "Admin")]
    public class ActivitiesController : ApiControllerBase
    {
        public IEventQueries EventQueries { get; set; }

        [GET("")]
        [Transaction()]
        public virtual HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, EventQueries.FindAll());
        }
    }
}