namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using Common.Infrastructure;
    using Common.Resources;
    using FileUpload;
    using ImageResizer;
    using Inventory.Model.Collaborators;

    [RoutePrefix("api/organizations/{organizationId}/files")]
    public class OrganizationsFilesController : ApiControllerBase
    {
        private readonly ICollaboratorService _collaboratorService;
        private readonly IFileStoreFactory _fileStoreFactory;

        public OrganizationsFilesController(ICollaboratorService collaboratorService, IFileStoreFactory fileStoreFactory)
        {
            _collaboratorService = collaboratorService;
            _fileStoreFactory = fileStoreFactory;
        }

        [GET("")]
        [Transaction]
        public virtual object Get(Guid organizationId)
        {
            _collaboratorService.Manager(CurrentUserId, new OwnerId(organizationId));

            var store = CreateImageStore(organizationId);
            var factory = CreateFactory(GetBaseFilesUrl(organizationId), organizationId);
            var result = factory.Create(store.GetFiles());
            return new {files = result};
        }

        [GET("{fileName}")]
        [Transaction]
        public virtual HttpResponseMessage Get(Guid organizationId, string fileName)
        {
            _collaboratorService.Manager(CurrentUserId, new OwnerId(organizationId));

            var store = CreateImageStore(organizationId);
            var stream = store.Get(fileName, 0);
            var result = new MemoryStream();

            var query = base.GetQueryParams();
            if (query.ContainsKey("maxwidth") || (query.ContainsKey("maxheight")))
            {
                var job = new ImageJob(stream, result, new ResizeSettings(Request.RequestUri.Query));
                job.Build();
            }
            else
            {
                stream.CopyTo(result);
            }
            result.Seek(0, SeekOrigin.Begin);

            var contentType = fileName.Substring(fileName.LastIndexOf('.') + 1);
            if (contentType == "jpg")
                contentType = "jpeg";

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(result);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/" + contentType);
            return response;
        }

        [POST("")]
        [Transaction]
        public virtual object Post(Guid organizationId)
        {
            _collaboratorService.Manager(CurrentUserId, new OwnerId(organizationId));

            var store = CreateImageStore(organizationId);
            var factory = CreateFactory(GetBaseFilesUrl(organizationId), organizationId);
            var handler = new BlueImpFileUploadHandler(store);
            var images = handler.Handle(HttpContext.Current.Request.Files);
            return new {files = factory.Create(images)};
        }

        [DELETE("{fileName}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(Guid organizationId, string fileName)
        {
            _collaboratorService.Manager(CurrentUserId, new OwnerId(organizationId));

            var store = CreateImageStore(organizationId);
            store.Remove(fileName);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        private string GetBaseFilesUrl(Guid orgId)
        {
            return String.Format(@"/api/organizations/{0}/files", orgId.ToString("N"));
        }

        private IFileStore CreateImageStore(Guid organizationId)
        {
            return _fileStoreFactory.GetOrganizations(organizationId);
        }

        private BlueImpFileUploadJsonResultFactory CreateFactory(string path, Guid organizationId)
        {
            var factory = new BlueImpFileUploadJsonResultFactory();
            factory.ImageUrl = path;
            factory.DeleteUrl = "/api/organizations/" + organizationId + "/files";
            return factory;
        }
    }
}