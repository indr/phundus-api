namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using FileUpload;
    using IdentityAccess.Projections;
    using IdentityAccess.Resources;

    [RoutePrefix("api/organizations/{organizationId}/files")]
    public class OrganizationsFilesController : ApiControllerBase
    {
        private readonly IMemberInRole _memberInRole;

        public OrganizationsFilesController(IMemberInRole memberInRole)
        {
            _memberInRole = memberInRole;
        }

        private string GetPath(Guid orgId)
        {
            return String.Format(@"~\Content\Uploads\Organizations\{0}", orgId.ToString("N"));
        }

        private string GetBaseFilesUrl(Guid orgId)
        {
            return String.Format(@"/Content/Uploads/Organizations/{0}", orgId.ToString("N"));
        }

        private ImageStore CreateImageStore(string path)
        {
            return new ImageStore(path);
        }

        private BlueImpFileUploadJsonResultFactory CreateFactory(string path, Guid organizationId)
        {
            var factory = new BlueImpFileUploadJsonResultFactory();
            factory.ImageUrl = path;
            factory.DeleteUrl = "/api/organizations/" + organizationId + "/files";
            return factory;
        }

        [GET("")]
        [Transaction]
        public virtual object Get(Guid organizationId)
        {
            _memberInRole.ActiveManager(organizationId, CurrentUserId);

            var path = GetPath(organizationId);
            var store = CreateImageStore(path);
            var factory = CreateFactory(GetBaseFilesUrl(organizationId), organizationId);
            var result = factory.Create(store.GetFiles());
            return new {files = result};
        }

        [POST("")]
        [Transaction]
        public virtual object Post(Guid organizationId)
        {
            _memberInRole.ActiveManager(organizationId, CurrentUserId);

            var path = GetPath(organizationId);
            var store = CreateImageStore(path);
            var factory = CreateFactory(GetBaseFilesUrl(organizationId), organizationId);
            var handler = new BlueImpFileUploadHandler(store);
            var images = handler.Handle(HttpContext.Current.Request.Files);
            return new {files = factory.Create(images)};
        }

        [DELETE("{fileName}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(Guid organizationId, string fileName)
        {
            _memberInRole.ActiveManager(organizationId, CurrentUserId);

            var path = GetPath(organizationId);
            var store = CreateImageStore(path);
            store.Delete(fileName);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}