namespace Phundus.Rest.Api
{
    using System;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using Integration.IdentityAccess;
    using Newtonsoft.Json;

    [RoutePrefix("api/username-check")]
    public class UsernameCheckController : ApiControllerBase
    {
        private readonly IUserQueries _userQueries;

        public UsernameCheckController(IUserQueries userQueries)
        {
            if (userQueries == null) throw new ArgumentNullException("userQueries");

            _userQueries = userQueries;
        }

        [POST("")]
        [AllowAnonymous]
        [Transaction]
        public virtual UsernameCheckPostOkResponseContent Post(UsernameCheckPostRequestContent requestContent)
        {
            var user = _userQueries.FindByUsername(requestContent.Username);

            return new UsernameCheckPostOkResponseContent
            {
                ValidUsername = user == null
            };
        }
    }

    public class UsernameCheckPostRequestContent
    {
        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public class UsernameCheckPostOkResponseContent
    {
        [JsonProperty("validUsername")]
        public bool ValidUsername { get; set; }
    }
}