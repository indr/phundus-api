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
                return new UsersCartGetOkResponseContent(null);
            return new UsersCartGetOkResponseContent(cart);
        }

        [DELETE("")]
        [Transaction]
        public virtual HttpResponseMessage Delete(Guid userGuid)
        {
            if (userGuid != CurrentUserGuid.Id)
                throw new ArgumentException("userGuid");

            Dispatch(new ClearCart(CurrentUserId));

            return NoContent();
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

        [PATCH("items/{itemGuid}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(Guid userGuid, Guid itemGuid,
            UsersCartPatchRequestContent requestContent)
        {
            if (userGuid != CurrentUserGuid.Id)
                throw new ArgumentException("userGuid");

            var command = new UpdateCartItem(CurrentUserId, CurrentUserGuid, itemGuid, requestContent.Quantity,
                requestContent.FromUtc, requestContent.ToUtc);
            Dispatch(command);

            return NoContent();
        }

        [DELETE("items/{itemGuid}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(Guid userGuid, Guid itemGuid)
        {
            if (userGuid != CurrentUserGuid.Id)
                throw new ArgumentException("userGuid");

            Dispatch(new RemoveCartItem(CurrentUserId, new CartItemGuid(itemGuid)));

            return NoContent();
        }
    }

    public class UsersCartPatchRequestContent
    {
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("fromUtc")]
        public DateTime FromUtc { get; set; }

        [JsonProperty("toUtc")]
        public DateTime ToUtc { get; set; }
    }

    public class UsersCartGetOkResponseContent
    {
        public UsersCartGetOkResponseContent(ICart cart)
        {
            if (cart == null)
            {
                Items = new List<CartItem>(0);
                return;
            }

            CartId = cart.CartId;
            CartGuid = cart.CartGuid;
            UserId = cart.UserId;
            UserGuid = cart.UserGuid;
            Items = cart.Items.Select(s => new CartItem
            {
                CartItemGuid = s.CartItemGuid,
                ArticleId = s.ArticleId,
                Text = s.Text,
                FromUtc = s.FromUtc,
                ToUtc = s.ToUtc,
                Quantity = s.Quantity,
                UnitPricePerWeek = s.UnitPricePerWeek,
                Days = s.Days,
                ItemTotal = s.ItemTotal,
                OwnerGuid = s.OwnerGuid,
                OwnerName = s.OwnerName
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
        [JsonProperty("cartItemGuid")]
        public Guid CartItemGuid { get; set; }

        [JsonProperty("articleId")]
        public int ArticleId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("fromUtc")]
        public DateTime FromUtc { get; set; }

        [JsonProperty("toUtc")]
        public DateTime ToUtc { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("unitPricePerWeek")]
        public decimal UnitPricePerWeek { get; set; }

        [JsonProperty("days")]
        public int Days { get; set; }

        [JsonProperty("itemTotal")]
        public decimal ItemTotal { get; set; }

        [JsonProperty("ownerGuid")]
        public Guid OwnerGuid { get; set; }

        [JsonProperty("ownerName")]
        public string OwnerName { get; set; }

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