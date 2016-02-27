namespace Phundus.Rest.Api.Account
{
    using System;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using IdentityAccess.Application;
    using Newtonsoft.Json;

    [RoutePrefix("api/account/change-password")]
    public class ChangePasswordController : ApiControllerBase
    {
        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(ChangePasswordPostRequestContent requestContent)
        {
            if (requestContent == null) throw new ArgumentNullException("requestContent");

            Dispatch(new ChangePassword(CurrentUserId, requestContent.OldPassword, requestContent.NewPassword));

            return NoContent();
        }
    }

    public class ChangePasswordPostRequestContent
    {
        [JsonProperty("oldPassword")]
        public string OldPassword { get; set; }

        [JsonProperty("newPassword")]
        public string NewPassword { get; set; }
    }
}