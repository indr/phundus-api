namespace Phundus.Rest.Api.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using Core.Shop.Orders.Commands;
    using Integration.Shop;
    using Newtonsoft.Json;

    [RoutePrefix("api/users/{userGuid}/cart")]
    public class UsersCartController : ApiControllerBase
    {
        private readonly ICartQueries _cartQueries;

        public UsersCartController(ICartQueries cartQueries)
        {
            if (cartQueries == null) throw new ArgumentNullException("cartQueries");
            _cartQueries = cartQueries;
        }

        [GET("")]
        [Transaction]
        public virtual UsersCartGetOkResponseContent Get(Guid userGuid)
        {
            if (userGuid != CurrentUserGuid.Id)
                throw new ArgumentException("userGuid");

            var cart = _cartQueries.FindByUserGuid(CurrentUserId, new UserGuid(userGuid));
            if (cart == null)
                throw new NotFoundException("Cart not found.");
            return new UsersCartGetOkResponseContent(cart);
        }

        [POST("items")]
        [Transaction]
        public virtual UsersCartItemsPostOkResponseContent Post(Guid userGuid, UsersCartItemsPostRequestContent requestContent)
        {
            if (userGuid != CurrentUserGuid.Id)
                throw new ArgumentException("userGuid");

            var command = new AddArticleToCart(CurrentUserId, CurrentUserGuid, new ArticleId(requestContent.ArticleId),
                requestContent.FromUtc, requestContent.ToUtc, requestContent.Quantity);
            Dispatch(command);

            return new UsersCartItemsPostOkResponseContent
            {
                CartItemId = command.ResultingCartItemGuid.Id
            };
        }

        [DELETE("items/{itemId}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(Guid userGuid, Guid itemId)
        {
            if (userGuid != CurrentUserGuid.Id)
                throw new ArgumentException("userGuid");

            Dispatch(new RemoveCartItem(CurrentUserId, new CartItemGuid(itemId)));

            return NoContent();
        }
    }

    public class UsersCartGetOkResponseContent
    {
        public UsersCartGetOkResponseContent(ICart cart)
        {
            CartId = cart.CartId;
            CartGuid = cart.CartGuid;
            UserId = cart.UserId;
            UserGuid = cart.UserGuid;
            Items = cart.Items.Select(s => new CartItem
            {
                ArticleId = s.ArticleId,
                Text = s.Text,
                FromUtc = s.FromUtc,
                ToUtc = s.ToUtc,
                Quantity = s.Quantity
            }).ToList();
        }

        public Guid UserGuid { get; set; }

        public int UserId { get; set; }

        public Guid CartGuid { get; set; }

        public int CartId { get; set; }

        [JsonProperty("items")]
        public IList<CartItem> Items { get; set; }
    }

    public class CartItem
    {
        public int ArticleId { get; set; }
        public string Text { get; set; }
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
        public int Quantity { get; set; }
    }

    public class UsersCartItemsPostOkResponseContent
    {
        [JsonProperty("cartItemId")]
        public Guid CartItemId { get; set; }
    }

    public class UsersCartItemsPostRequestContent
    {
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }

        [JsonProperty("fromUtc")]
        public DateTime FromUtc { get; set; }

        [JsonProperty("toUtc")]
        public DateTime ToUtc { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}