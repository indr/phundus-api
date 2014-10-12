namespace Phundus.Rest.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.Ddd;

    [RoutePrefix("api/notifications")]
    [Authorize(Roles = "Admin")]
    public class NotificationsController : ApiControllerBase
    {
        public IStoredEventRepository StoredEventRepository { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get()
        {
            var result = StoredEventRepository.FindAll();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}