namespace Phundus.Rest.Api.Account
{
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using ContentObjects;
    using Core.IdentityAndAccess.Users.Commands;

    [RoutePrefix("api/account/reset-password")]
    [AllowAnonymous]
    public class ResetPasswordController : ApiControllerBase
    {
        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(ResetPasswordPostRequestContent requestContent)
        {
            Dispatch(new ResetPassword(requestContent.EmailAddress));
            return NoContent();
        }
    }
}