namespace Phundus.Rest.Api
{
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Common.Resources;
    using IdentityAccess.Application;
    using Newtonsoft.Json;

    [RoutePrefix("api/email-address-check")]
    public class EmailAddressCheckController : ApiControllerBase
    {
        private readonly IUserQueryService _userQueryService;

        public EmailAddressCheckController(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
        }

        [POST("")]
        [AllowAnonymous]
        public virtual EmailAddressCheckPostOkResponseContent Post(EmailAddressCheckPostRequestContent requestContent)
        {
            var isTaken = _userQueryService.IsEmailAddressTaken(requestContent.EmailAddress);

            return new EmailAddressCheckPostOkResponseContent
            {
                ValidEmailAddress = !isTaken
            };
        }
    }

    public class EmailAddressCheckPostRequestContent
    {
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
    }

    public class EmailAddressCheckPostOkResponseContent
    {
        [JsonProperty("validEmailAddress")]
        public bool ValidEmailAddress { get; set; }
    }
}