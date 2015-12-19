namespace Phundus.Rest.Api
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;

    [RoutePrefix("api/users")]
    public class UsersController : ApiControllerBase
    {
        [GET("")]
        public virtual HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.NoContent, new List<object>());
        }

        [GET("{userId}")]
        public virtual HttpResponseMessage Get(string userId)
        {
            return CreateNotFoundResponse("Der Benutzer mit der Id {0} konnte nicht gefunden werden.", userId);
        }
    }
}