namespace Phundus.Specs.ContentTypes
{
    using System;
    using Newtonsoft.Json;

    public class OrganizationsApplicationsPostOkResponseContent
    {
        [JsonProperty("applicationId")]
        public Guid ApplicationId { get; set; }
    }
}