namespace Phundus.Rest.Api.Account
{
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using IdentityAccess.Application;
    using Newtonsoft.Json;

    [RoutePrefix("api/account/reset-password")]
    public class ResetPasswordController : ApiControllerBase
    {
        [POST("")]
        [AllowAnonymous]
        public virtual HttpResponseMessage Post(ResetPasswordPostRequestContent requestContent)
        {
            Dispatch(new ResetPassword(requestContent.EmailAddress));

            return Accepted();
        }
    }

    public class ResetPasswordPostRequestContent
    {
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
    }
}