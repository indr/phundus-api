namespace Phundus.Rest.Api
{
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using Common;    
    using Newtonsoft.Json;

    [RoutePrefix("api/maintenance")]
    [Authorize(Roles = "Admin")]
    public class MaintenanceController : ApiControllerBase
    {
        [PATCH("")]
        public virtual HttpResponseMessage Patch(MaintenancePatchRequestContent requestContent)
        {
            if (requestContent.InMaintenance.HasValue)
                Config.InMaintenance = requestContent.InMaintenance.Value;
            return NoContent();
        }
    }

    public class MaintenancePatchRequestContent
    {
        [JsonProperty("inMaintenance")]
        public bool? InMaintenance { get; set; }
    }
}