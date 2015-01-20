namespace Phundus.Rest.Api.Inventory
{
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.Inventory.Application;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Reservations;

    [RoutePrefix("api/organizations/{organizationId}/articles/{articleId}/reservations")]
    public class ArticlesReservationsController : ApiControllerBase
    {
        public IReservationQueryService ReservationQueryService { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId, int articleId)
        {
            var result = ReservationQueryService.AllReservationDataByArticle(new OrganizationId(organizationId),
                new ArticleId(articleId));
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [GET("{reservationId}")]
        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId, int articleId, string reservationId)
        {
            var result = ReservationQueryService.ReservationDataOfId(new OrganizationId(organizationId),
                new ArticleId(articleId), new ReservationId(reservationId));
            if (result == null)
                Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Die Reservation mit der Id " + reservationId + " ist nicht vorhanden.");

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}