namespace Phundus.Rest.Api
{
    using System;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using Newtonsoft.Json;

    [RoutePrefix("api/email-address-check")]
    public class EmailAddressCheckController : ApiControllerBase
    {
        private readonly IUserQueries _userQueries;

        public EmailAddressCheckController(IUserQueries userQueries)
        {
            if (userQueries == null) throw new ArgumentNullException("userQueries");

            _userQueries = userQueries;
        }

        [POST("")]
        [AllowAnonymous]
        [Transaction]
        public virtual EmailAddressCheckPostOkResponseContent Post(EmailAddressCheckPostRequestContent requestContent)
        {
            var isTaken = _userQueries.IsEmailAddressTaken(requestContent.EmailAddress);

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