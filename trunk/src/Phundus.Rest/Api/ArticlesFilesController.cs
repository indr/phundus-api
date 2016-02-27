namespace Phundus.Rest.Api
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using FileUpload;
    using Inventory.Application;
    using Inventory.Projections;
    using Newtonsoft.Json;

    [RoutePrefix("api/articles/{articleId}/files")]
    public class ArticlesFilesController : ApiControllerBase
    {
        private readonly IImagesQueries _imagesQueries;

        public ArticlesFilesController(IImagesQueries imagesQueries)
        {
            if (imagesQueries == null) throw new ArgumentNullException("imagesQueries");
            _imagesQueries = imagesQueries;
        }

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

        private BlueImpFileUploadJsonResultFactory CreateFactory(string path, int articleId)
        {
            var factory = new BlueImpFileUploadJsonResultFactory();
            factory.ImageUrl = path;
            factory.DeleteUrl = "/api/articles/" + articleId + "/files";
            return factory;
        }

        [GET("")]
        [Transaction]
        public virtual object Get(int articleId)
        {
            // TODO: Auth filtering
            var factory = CreateFactory(GetBaseFilesUrl(articleId), articleId);
            var images = _imagesQueries.ByArticle(articleId);
            var result = factory.Create(images);
            return new {files = result};
        }

        [POST("")]
        [Transaction]
        public virtual object Post(int articleId)
        {
            var path = GetPath(articleId);
            var store = CreateImageStore(path);
            var factory = CreateFactory(GetBaseFilesUrl(articleId), articleId);
            var handler = new BlueImpFileUploadHandler(store);
            var images = handler.Handle(HttpContext.Current.Request.Files);
            foreach (var each in images)
            {
                var command = new AddImage(CurrentUserId, new ArticleShortId(articleId), Path.GetFileName(each.FileName),
                    each.Type,
                    each.Length);
                Dispatcher.Dispatch(command);
                each.Id = command.ResultingImageId;
            }
            return new {files = factory.Create(images)};
        }

        [PATCH("{fileName}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(int articleId, string fileName,
            ArticlesFilesPatchRequestContent requestContent)
        {
            Dispatch(new SetPreviewImage(CurrentUserId, new ArticleShortId(articleId), fileName));

            return NoContent();
        }

        [DELETE("{fileName}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(int articleId, string fileName)
        {
            var path = GetPath(articleId);
            var store = CreateImageStore(path);
            Dispatcher.Dispatch(new RemoveImage(CurrentUserId, new ArticleShortId(articleId), fileName));
            store.Delete(fileName);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }

    public class ArticlesFilesPatchRequestContent
    {
        [JsonProperty("isPreview")]
        public bool IsPreview { get; set; }
    }
}