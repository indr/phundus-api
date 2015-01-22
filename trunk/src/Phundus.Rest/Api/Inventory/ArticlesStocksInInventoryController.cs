namespace Phundus.Rest.Api.Inventory
{
    using System;
    using System.Data;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.Inventory.Application;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Application.Data;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;

    [RoutePrefix("api/organizations/{organizationId}/articles/{articleId}/stocks/{stockId}/in-inventory")]
    [Authorize(Roles = "Admin")]
    public class ArticlesStocksInInventoryController : ApiControllerBase
    {
        public IQuantitiesInInventoryQueryService QuantitiesInInventoryQueryService { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId, int articleId, string stockId)
        {
            var result = QuantitiesInInventoryQueryService.AllQuantitiesInInventoryByArticleId(organizationId, articleId, stockId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(int organizationId, int articleId, string stockId, QuantityInInventoryData data)
        {
            var command = new ChangeQuantityInInventory(new UserId(CurrentUserId), new OrganizationId(organizationId), 
                new ArticleId(articleId), new StockId(stockId), data.Change, data.AsOfUtc, data.Comment);

            Dispatch(command);

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}