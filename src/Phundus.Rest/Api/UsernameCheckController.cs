namespace Phundus.Rest.Api
{
    using System;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Integration.IdentityAccess;
    using Newtonsoft.Json;

    [RoutePrefix("api/username-check")]
    public class UsernameCheckController : ApiControllerBase
    {
        private readonly IUsersQueries _usersQueries;

        public UsernameCheckController(IUsersQueries usersQueries)
        {
            if (usersQueries == null) throw new ArgumentNullException("usersQueries");

            _usersQueries = usersQueries;
        }

        [POST("")]
        [AllowAnonymous]        
        public virtual UsernameCheckPostOkResponseContent Post(UsernameCheckPostRequestContent requestContent)
        {
            var user = _usersQueries.FindByUsername(requestContent.Username);

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