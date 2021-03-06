﻿namespace Phundus.Rest.Api
{
    using System;
    using System.Reflection;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Common;
    using Common.Resources;
    using ContentObjects;    

    [RoutePrefix("api/status")]
    public class StatusController : ApiControllerBase
    {
        [GET("")]
        [AllowAnonymous]
        public virtual StatusGetOkResponseContent Get()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version.ToString(3);
            var revision = assembly.GetName().Version.Revision;
            var serverVersion = String.Format("{0} (build {1})", version, revision);
            var inMaintenance = Config.InMaintenance;

            return new StatusGetOkResponseContent
            {
                ServerDateTimeUtc = DateTime.UtcNow,
                ServerUrl = Config.ServerUrl,
                ServerVersion = serverVersion,
                InMaintenance = inMaintenance
            };
        }
    }
}