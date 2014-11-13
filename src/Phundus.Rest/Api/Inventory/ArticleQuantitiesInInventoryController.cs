namespace Phundus.Rest.Api.Inventory
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.Inventory.Application;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Application.Data;

    [RoutePrefix("api/organizations/{organizationId}/articles/{articleId}/quantities/in-inventory")]
    [Authorize(Roles = "Admin")]
    public class ArticleQuantitiesInInventoryController : ApiControllerBase
    {
        public IQuantitiesInInventoryQueryService QuantitiesInInventoryQueryService { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId, int articleId)
        {
            var result = QuantitiesInInventoryQueryService.AllQuantitiesInInventoryByArticleId();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(int organizationId, int articleId, QuantityInInventoryData data)
        {
            if (data.Change == 0)
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Change must be non zero");

            var command = new ChangeQuantityInInventory(CurrentUserId, organizationId, articleId, articleId.ToString(),
                data.Change, DateTime.UtcNow, data.Comment);

            Dispatch(command);

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}