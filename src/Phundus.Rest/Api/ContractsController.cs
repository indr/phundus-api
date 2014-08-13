namespace Phundus.Rest.Api
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;

    [RoutePrefix("api/contracts")]
    public class ContractsController : ApiControllerBase
    {
        [GET("")]
        public virtual HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.NoContent, new List<object>());
        }

        [GET("{contractId}")]
        public virtual HttpResponseMessage Get(int contractId)
        {
            return CreateNotFoundResponse("Der Vertrag mit der Id {0} konnte nicht gefunden werden.", contractId);
        }
    }
}