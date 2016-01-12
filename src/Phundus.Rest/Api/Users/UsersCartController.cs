namespace Phundus.Rest.Api.Users
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using Core.Shop.Orders.Commands;
    using Newtonsoft.Json;

    [RoutePrefix("api/users/{userId}/cart")]
    public class UsersCartController : ApiControllerBase
    {
        [GET("")]
        [Transaction]
        public virtual UsersCartGetOkResponseContent Get(int userId)
        {
            if (userId != CurrentUserId.Id)
                throw new ArgumentException("userId");

            return new UsersCartGetOkResponseContent();
        }

        [POST("items")]
        [Transaction]
        public virtual UsersCartItemsPostOkResponseContent Post(int userId, UsersCartItemsPostRequestContent requestContent)
        {
            if (userId != CurrentUserId.Id)
                throw new ArgumentException("userId");

            var command = new AddArticleToCart(CurrentUserId, new ArticleId(requestContent.ArticleId),
                requestContent.FromUtc, requestContent.ToUtc, requestContent.Quantity);
            Dispatch(command);

            return new UsersCartItemsPostOkResponseContent
            {
                CartItemId = command.ResultingCartItemId.Id
            };
        }

        [DELETE("items/{itemId}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(int userId, Guid itemId)
        {
            if (userId != CurrentUserId.Id)
                throw new ArgumentException("userId");

            Dispatch(new RemoveCartItem(CurrentUserId, new CartItemId(itemId)));

            return NoContent();
        }
    }

    public class UsersCartGetOkResponseContent
    {
        public UsersCartGetOkResponseContent()
        {
            Items = new List<CartItem>();
        }

        [JsonProperty("items")]
        public IList<CartItem> Items { get; set; }
    }

    public class CartItem
    {
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