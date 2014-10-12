namespace Phundus.Rest.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Notifications;

    [RoutePrefix("api/notifications")]
    [Authorize(Roles = "Admin")]
    public class NotificationsController : ApiControllerBase
    {
        public INotificationLogFactory NotificationLogFactory { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get()
        {
            var result = NotificationLogFactory.CreateCurrentNotificationLog();
            if (result == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "");

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [GET("{notificationId}")]
        public virtual HttpResponseMessage Get(string notificationId)
        {
            var result = NotificationLogFactory.CreateNotificationLog(notificationId);
            if (result == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "");

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}