namespace Phundus.Rest.Api.Inventory
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.Inventory.Application;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Application.Data;
    using Core.Inventory.Domain.Model.Catalog;

    [RoutePrefix("api/organizations/{organizationId}/articles/{articleId}/stocks")]
    [Authorize(Roles = "Admin")]
    public class OrganizationsArticlesStocksController : ApiControllerBase
    {
        public IStocksQueryService StocksQueryService { get; set; }

        [GET("")]
        public virtual HttpResponseMessage Get(int organizationId, int articleId)
        {
            var result = StocksQueryService.AllStocksByArticleId(articleId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [POST("")]
        public virtual HttpResponseMessage Post(int organizationId, int articleId, StockData data)
        {
            var command = new CreateStock(new UserId(CurrentUserId), new OrganizationId(organizationId), new ArticleId(data.ArticleId));
            Dispatch(command);

            return Request.CreateResponse(HttpStatusCode.Created, command.ResultingStockId);
        }
    }
}