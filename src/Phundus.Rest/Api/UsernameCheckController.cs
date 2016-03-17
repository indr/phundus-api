namespace Phundus.Rest.Api
{
    using System;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Resources;
    using IdentityAccess.Application;
    using Newtonsoft.Json;

    [RoutePrefix("api/username-check")]
    public class UsernameCheckController : ApiControllerBase
    {
        private readonly IUserQueryService _userQueryService;

        public UsernameCheckController(IUserQueryService userQueryService)
        {
            if (userQueryService == null) throw new ArgumentNullException("userQueryService");

            _userQueryService = userQueryService;
        }

        [POST("")]
        [AllowAnonymous]        
        public virtual UsernameCheckPostOkResponseContent Post(UsernameCheckPostRequestContent requestContent)
        {
            var user = _userQueryService.FindByUsername(requestContent.Username);

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