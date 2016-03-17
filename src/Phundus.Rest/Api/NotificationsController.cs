namespace Phundus.Rest.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Notifications;
    using Common.Resources;

    [RoutePrefix("api/notifications")]
    [Authorize(Roles = "Admin")]
    public class NotificationsController : ApiControllerBase
    {
        private readonly INotificationLogFactory _notificationLogFactory;

        public NotificationsController(INotificationLogFactory notificationLogFactory)
        {
            _notificationLogFactory = notificationLogFactory;
        }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get()
        {
            var result = _notificationLogFactory.CreateCurrentNotificationLog();
            if (result == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "");

            return Ok(result);
        }

        [GET("{notificationId}")]
        [Transaction]
        public virtual HttpResponseMessage Get(string notificationId)
        {
            var result = _notificationLogFactory.CreateNotificationLog(notificationId);
            if (result == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "");

            return Ok(result);
        }
    }
}