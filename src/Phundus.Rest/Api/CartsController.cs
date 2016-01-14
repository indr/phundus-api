namespace Phundus.Rest.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Newtonsoft.Json;
    using Phundus.Shop.Orders.Commands;
    using Users;

    [RoutePrefix("api/carts")]
    public class CartsController : ApiControllerBase
    {
        [GET("{userGuid}")]
        [Transaction]
        public virtual CartsGetOkResponseContent Get(Guid userGuid)
        {
            return new CartsGetOkResponseContent();
        }

        [DELETE("")]
        [Transaction]
        public virtual HttpResponseMessage Delete()
        {
            Dispatcher.Dispatch(new ClearCart(CurrentUserId));

            return NoContent();
        }
    }

    public class CartsGetOkResponseContent
    {
        public CartsGetOkResponseContent()
        {
            Items = new List<CartItem>();
        }

        [JsonProperty("items")]
        public IList<CartItem> Items { get; set; }
    }
}