namespace Phundus.Rest.Api.Shop
{
    using System;
    using System.Linq;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using ContentObjects;
    using Phundus.Shop.Queries;

    [RoutePrefix("api/shop/items")]
    public class ShopItemsController : ApiControllerBase
    {
        private readonly IItemQueries _itemQueries;

        public ShopItemsController(IItemQueries itemQueries)
        {
            if (itemQueries == null) throw new ArgumentNullException("itemQueries");
            _itemQueries = itemQueries;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<ShopQueryItem> Get(Guid? lessorId, string q)
        {
            var results = _itemQueries.Query(q);

            return new QueryOkResponseContent<ShopQueryItem>(results.Select(s => new ShopQueryItem
            {
                ArticleId = s.Id,
                Name = s.Name
            }));
        }
    }
}