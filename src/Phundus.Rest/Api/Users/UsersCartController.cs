namespace Phundus.Rest.Api.Users
{
    using System;    
    using System.Collections.Generic;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Newtonsoft.Json;

    [RoutePrefix("api/users/{userGuid}/cart")]
    public class UsersCartController : ApiControllerBase
    {
        [GET("")]
        [Transaction]
        public virtual UsersCartGetOkResponseContent Get(Guid userGuid)
        {
            return new UsersCartGetOkResponseContent();
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
}