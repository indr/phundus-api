namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using FileUpload;
    using Inventory.Articles.Commands;
    using Inventory.Queries;

    [RoutePrefix("api/organizations/{organizationId}/articles/{articleId}/files")]
    public class OrganizationsArticlesFilesController : ApiControllerBase
    {
        public IImageQueries ImageQueries { get; set; }

        private string GetPath(int articleId)
        {
            return String.Format(@"~\Content\Images\Articles\{0}", articleId);
        }

        private string GetBaseFilesUrl(int articleId)
        {
            return String.Format(@"/Content/Images/Articles/{0}", articleId);
        }

        private ImageStore CreateImageStore(string path)
        {
            return new ImageStore(path);
        }

        private BlueImpFileUploadJsonResultFactory CreateFactory(string path, Guid organizationId, int articleId)
        {
            var factory = new BlueImpFileUploadJsonResultFactory();
            factory.ImageUrl = path;
            factory.DeleteUrl = "/api/organizations/" + organizationId + "/articles/" + articleId + "/files";
            return factory;
        }

        [GET("")]
        [Transaction]
        public virtual object Get(Guid organizationId, int articleId)
        {
            // TODO: Auth filtering
            var factory = CreateFactory(GetBaseFilesUrl(articleId), organizationId, articleId);
            var images = ImageQueries.ByArticle(articleId);
            var result = factory.Create(images);
            return new {files = result};
        }

        [POST("")]
        [Transaction]
        public virtual object Post(Guid organizationId, int articleId)
        {
            var path = GetPath(articleId);
            var store = CreateImageStore(path);
            var factory = CreateFactory(GetBaseFilesUrl(articleId), organizationId, articleId);
            var handler = new BlueImpFileUploadHandler(store);
            var images = handler.Handle(HttpContext.Current.Request.Files);
            foreach (var each in images)
            {
                var command = new AddImage(CurrentUserGuid, new ArticleId(articleId), each.FileName, each.Type,
                    each.Length);
                Dispatcher.Dispatch(command);
                each.Id = command.ResultingImageId;
            }
            return new {files = factory.Create(images)};
        }

        [DELETE("{fileName}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(Guid organizationId, int articleId, string fileName)
        {
            var path = GetPath(articleId);
            var store = CreateImageStore(path);
            Dispatcher.Dispatch(new RemoveImage(CurrentUserGuid, new ArticleId(articleId), fileName));
            store.Delete(fileName);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}