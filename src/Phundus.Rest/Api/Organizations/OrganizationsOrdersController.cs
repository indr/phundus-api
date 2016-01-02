namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using Core.Shop.Orders;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Queries;

    [RoutePrefix("api/organizations/{organizationId}/orders")]
    public class OrganizationsOrdersController : ApiControllerBase
    {
        public IOrderQueries OrderQueries { get; set; }

        public IUserQueries UserQueries { get; set; }

        public IPdfStore PdfStore { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get(Guid organizationId)
        {
            var result = OrderQueries.ManyByOrganizationId(organizationId, CurrentUserId);
            return Request.CreateResponse(HttpStatusCode.OK, Map<ICollection<OrderDoc>>(result));
        }

        [GET("{orderId:int}")]
        [Transaction]
        public virtual HttpResponseMessage Get(Guid organizationId, int orderId)
        {
            var result = OrderQueries.SingleByOrderIdAndOrganizationId(orderId, organizationId, CurrentUserId);
            if (result == null)
                return CreateNotFoundResponse("Die Bestellung mit der Id {0} konnte nicht gefunden werden.", orderId);

            return Request.CreateResponse(HttpStatusCode.OK, Map<OrderDetailDoc>(result));
        }

        [GET("{orderId:int}.pdf")]
        [Transaction]
        public virtual HttpResponseMessage GetPdf(Guid organizationId, int orderId)
        {
            var stream = PdfStore.GetOrderPdf(orderId, CurrentUserId);
            if (stream == null)
                return CreateNotFoundResponse("Die Bestellung mit der Id {0} konnte nicht gefunden werden.", orderId);

            return CreatePdfResponse(stream, string.Format("Bestellung-{0}.pdf", orderId));
        }

        [PATCH("{orderId}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(Guid organizationId, int orderId, OrderPatchDoc doc)
        {
            if (doc.Status == "Rejected")
                Dispatch(new RejectOrder {InitiatorId = CurrentUserId, OrderId = orderId});
            else if (doc.Status == "Approved")
                Dispatch(new ApproveOrder {InitiatorId = CurrentUserId, OrderId = orderId});
            else if (doc.Status == "Closed")
                Dispatch(new CloseOrder {InitiatorId = CurrentUserId, OrderId = orderId});
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Unbekannter Status \"" + doc.Status + "\"");

            return Get(organizationId, orderId);
        }

        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(Guid organizationId, OrdersPostDoc doc)
        {
            int userId;
            if (!Int32.TryParse(doc.UserName, out userId))
            {
                var user = UserQueries.ByUserName(doc.UserName);
                if (user == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        string.Format("Der Benutzer mit der E-Mail-Adresse \"{0}\" konnte nicht gefunden werden.",
                            doc.UserName));

                userId = user.Id;
            }

            var command = new CreateEmptyOrder
            {
                InitiatorId = CurrentUserId,
                OrganizationId = organizationId,
                UserId = userId
            };

            Dispatcher.Dispatch(command);

            return Get(organizationId, command.OrderId);
        }
    }
}