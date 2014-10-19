namespace Phundus.Rest.Api.Diagnostics
{
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Castle.Windsor;
    using Core.Dashboard.Querying;
    using Core.IdentityAndAccess.Queries;

    [RoutePrefix("api/diagnostics/events")]
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous] // TODO: AllowAnonymous entfernen
    public class EventsController : ApiControllerBase
    {
        public IEventsQueries EventsQueries { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, EventsQueries.FindAll());
        }
    }
}