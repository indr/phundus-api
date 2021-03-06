﻿namespace Phundus.Rest.Api.Account
{
    using System;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Common.Resources;
    using IdentityAccess.Application;
    using Newtonsoft.Json;

    [RoutePrefix("api/account/change-email-address")]
    public class ChangeEmailAddressController : ApiControllerBase
    {
        [POST("")]
        public virtual HttpResponseMessage Post(ChangeEmailAddressPostRequestContent requestContent)
        {
            if (requestContent == null) throw new ArgumentNullException("requestContent");

            Dispatch(new ChangeEmailAddress(CurrentUserId, CurrentUserId, requestContent.Password, requestContent.NewEmailAddress));

            return NoContent();
        }
    }

    public class ChangeEmailAddressPostRequestContent
    {
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("newEmailAddress")]
        public string NewEmailAddress { get; set; }
    }
}