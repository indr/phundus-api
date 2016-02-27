namespace Phundus.Rest.Api.Account
{
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using IdentityAccess.Application;
    using Newtonsoft.Json;

    [RoutePrefix("api/account/reset-password")]
    public class ResetPasswordController : ApiControllerBase
    {
        [POST("")]
        [AllowAnonymous]
        [Transaction]
        public virtual HttpResponseMessage Post(ResetPasswordPostRequestContent requestContent)
        {
            Dispatch(new ResetPassword(requestContent.EmailAddress));

            return NoContent();
        }
    }

    public class ResetPasswordPostRequestContent
    {
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
    }
}