namespace Phundus.Rest.Api.Diagnostics
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Web.Hosting;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;

    [RoutePrefix("api/diagnostics/schema-update")]
    [Authorize(Roles = "Admin")]
    public class SchemaUpdateController : ApiControllerBase
    {
        [GET("")]
        public virtual HttpResponseMessage Get()
        {
            var fileName = HostingEnvironment.MapPath(@"~\App_Data\SchemaUpdate.sql");
            if ((fileName == null) || (!File.Exists(fileName)))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "");

            var content = File.OpenText(fileName).ReadToEnd();
            if (String.IsNullOrWhiteSpace(content))
                return Request.CreateResponse(HttpStatusCode.NoContent, SchemaUpdateRepresentation.Empty);

            var representation = new SchemaUpdateRepresentation();
            representation.DateTimeUtc = File.GetLastWriteTimeUtc(fileName);
            representation.Script = content;
            return Request.CreateResponse(HttpStatusCode.OK, representation);
        }
    }

    public class SchemaUpdateRepresentation
    {
        public DateTime? DateTimeUtc { get; set; }
        public string Script { get; set; }

        public static SchemaUpdateRepresentation Empty { get { return new SchemaUpdateRepresentation(); } }
    }
}