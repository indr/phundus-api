namespace Phundus.Rest.Api
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
            if (!File.Exists(fileName))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "");

            var content = File.OpenText(fileName).ReadToEnd();
            if (String.IsNullOrWhiteSpace(content))
                return Request.CreateResponse(HttpStatusCode.NoContent);

            return Request.CreateResponse(HttpStatusCode.OK, content);
        }
    }
}