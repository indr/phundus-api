namespace Phundus.Rest.Api.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using Integration.Shop;
    using Newtonsoft.Json;
    using Phundus.Shop.Orders.Commands;

    [RoutePrefix("api/users/{userId}/cart")]
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
        public virtual UsersCartGetOkResponseContent Get(Guid userId)
        {
            if (userId != CurrentUserId.Id)
                throw new ArgumentException("userId");

            var cart = _cartQueries.FindByUserGuid(CurrentUserId, new UserGuid(userId));
            if (cart == null)
                return new UsersCartGetOkResponseContent(null);
            return new UsersCartGetOkResponseContent(cart);
        }

        [DELETE("")]
        [Transaction]
        public virtual HttpResponseMessage Delete(Guid userId)
        {
            if (userId != CurrentUserId.Id)
                throw new ArgumentException("userId");

            Dispatch(new ClearCart(CurrentUserId));

            return NoContent();
        }

        [POST("items")]
        [Transaction]
        public virtual UsersCartItemsPostOkResponseContent Post(Guid userId,
            UsersCartItemsPostRequestContent requestContent)
        {
            if (userId != CurrentUserId.Id)
                throw new ArgumentException("userId");

            if ((requestContent.Amount > 0) && (!String.IsNullOrEmpty(requestContent.Begin)) &&
                (!String.IsNullOrEmpty(requestContent.End)))
            {
                requestContent.Quantity = requestContent.Amount;
                requestContent.FromUtc = DateTime.Parse(requestContent.Begin).ToLocalTime().Date.ToUniversalTime();
                requestContent.ToUtc =
                    DateTime.Parse(requestContent.End).ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime();
            }

            var command = new AddArticleToCart(CurrentUserId, new ArticleId(requestContent.ArticleId),
                requestContent.FromUtc, requestContent.ToUtc, requestContent.Quantity);
            Dispatch(command);

            return new UsersCartItemsPostOkResponseContent
            {
                CartItemId = command.ResultingCartItemGuid.Id
            };
        }

        [PATCH("items/{itemId}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(Guid userId, Guid itemId,
            UsersCartPatchRequestContent requestContent)
        {
            if (userId != CurrentUserId.Id)
                throw new ArgumentException("userId");

            var command = new UpdateCartItem(CurrentUserId, itemId, requestContent.Quantity,
                requestContent.FromUtc, requestContent.ToUtc);
            Dispatch(command);

            return NoContent();
        }

        [DELETE("items/{itemId}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(Guid userId, Guid itemId)
        {
            if (userId != CurrentUserId.Id)
                throw new ArgumentException("userId");

            Dispatch(new RemoveCartItem(CurrentUserId, new CartItemGuid(itemId)));

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

            CartGuid = cart.CartGuid;
            UserGuid = cart.UserGuid;
            Items = cart.Items.Select(s => new CartItem
            {
                CartItemId = s.CartItemGuid,
                ArticleId = s.ArticleId,
                Text = s.Text,
                FromUtc = s.FromUtc,
                ToUtc = s.ToUtc,
                Quantity = s.Quantity,
                UnitPricePerWeek = s.UnitPricePerWeek,
                Days = s.Days,
                ItemTotal = s.ItemTotal,
                OwnerId = s.OwnerGuid,
                OwnerName = s.OwnerName
            }).ToList();
        }

        [JsonProperty("userId")]
        public Guid UserGuid { get; set; }
        
        [JsonProperty("cartId")]
        public Guid CartGuid { get; set; }

        [JsonProperty("items")]
        public IList<CartItem> Items { get; set; }
    }

    public class CartItem
    {
        [JsonProperty("cartItemId")]
        public Guid CartItemId { get; set; }

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

        [JsonProperty("ownerId")]
        public Guid OwnerId { get; set; }

        [JsonProperty("ownerName")]
        public string OwnerName { get; set; }
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

        public string Begin { get; set; }
        public string End { get; set; }
        public int Amount { get; set; }
    }

    public class UsersCartItemsPostOkResponseContent
    {
        [JsonProperty("cartItemId")]
        public Guid CartItemId { get; set; }
    }
}