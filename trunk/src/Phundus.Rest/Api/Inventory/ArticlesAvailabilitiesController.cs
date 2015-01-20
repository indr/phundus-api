namespace Phundus.Rest.Api.Inventory
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.Inventory.Application;

    [RoutePrefix("api/organizations/{organizationId}/articles/{articleId}/availabilities")]
    [Authorize(Roles = "Admin")]
    public class ArticlesAvailabilitiesController : ApiControllerBase
    {
        public IQuantitiesAvailableQueryService QuantitiesAvailableQueryService { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId, int articleId)
        {
            var result = QuantitiesAvailableQueryService.AllQuantitiesAvailableByArticleId(organizationId, articleId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}