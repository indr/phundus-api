namespace Phundus.Rest.Api
{
    using System;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Integration.IdentityAccess;
    using Newtonsoft.Json;

    [RoutePrefix("api/email-address-check")]
    public class EmailAddressCheckController : ApiControllerBase
    {
        private readonly IUsersQueries _usersQueries;

        public EmailAddressCheckController(IUsersQueries usersQueries)
        {
            if (usersQueries == null) throw new ArgumentNullException("usersQueries");

            _usersQueries = usersQueries;
        }

        [POST("")]
        [AllowAnonymous]
        [Transaction]
        public virtual EmailAddressCheckPostOkResponseContent Post(EmailAddressCheckPostRequestContent requestContent)
        {
            var isTaken = _usersQueries.IsEmailAddressTaken(requestContent.EmailAddress);

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