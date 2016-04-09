namespace Phundus.Inventory.Resources
{
    using System;
    using System.Net.Http;
    using Application;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Common.Domain.Model;
    using Common.Resources;
    using Newtonsoft.Json;

    [RoutePrefix("api/inventory/products/{productId}/tags")]
    public class ProductsTagsController : ApiControllerBase
    {
        [POST("")]
        public virtual HttpResponseMessage Post(Guid productId, PostRequestContent rq)
        {
            Dispatch(new TagProductCommand(CurrentUserId, new ArticleId(productId), rq.Name));

            return NoContent();
        }

        [DELETE("{name}")]
        public virtual HttpResponseMessage Delete(Guid productId, string name)
        {
            Dispatch(new UntagProductCommand(CurrentUserId, new ArticleId(productId), name));

            return NoContent();
        }

        public class PostRequestContent
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }
}