namespace Phundus.Rest.Api.Account
{
    using System;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Users.Commands;
    using Newtonsoft.Json;

    [RoutePrefix("api/account/change-email-address")]
    public class ChangeEmailAddressController : ApiControllerBase
    {
        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(ChangeEMailAddressPostRequestContent requestContent)
        {
            if (requestContent == null) throw new ArgumentNullException("requestContent");

            Dispatch(new ChangeEmailAddress(CurrentUserId, requestContent.NewEmailAddress));

            return NoContent();
        }
    }

    public class ChangeEMailAddressPostRequestContent
    {
        [JsonProperty("newEmailAddress")]
        public string NewEmailAddress { get; set; }
    }

}