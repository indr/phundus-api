namespace Phundus.Rest.Api.Account
{
    using System;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using IdentityAccess.Application;
    using Newtonsoft.Json;

    [RoutePrefix("api/account/validate")]
    public class ValidateController : ApiControllerBase
    {
        [POST("")]
        [AllowAnonymous]
        public virtual HttpResponseMessage Post(ValidatePostRequestContent requestContent)
        {
            if (requestContent == null) throw new ArgumentNullException("requestContent");

            Dispatch(new ValidateKey(requestContent.Key));

            return NoContent();
        }
    }

    public class ValidatePostRequestContent
    {
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}