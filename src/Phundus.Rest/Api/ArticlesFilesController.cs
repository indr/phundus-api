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
    using Common.Resources;
    using FileUpload;
    using Inventory.Application;
    using Inventory.Projections;
    using Newtonsoft.Json;

    [RoutePrefix("api/articles/{articleId}/files")]
    public class ArticlesFilesController : ApiControllerBase
    {
        private readonly IArticleFileQueryService _articleFileQueryService;

        public ArticlesFilesController(IArticleFileQueryService articleFileQueryService)
        {
            if (articleFileQueryService == null) throw new ArgumentNullException("articleFileQueryService");

            _articleFileQueryService = articleFileQueryService;
        }

        private string GetPath(ArticleId articleId)
        {
            return String.Format(@"~\Content\Images\Articles\{0}", articleId.Id.ToString("D"));
        }

        private string GetBaseFilesUrl(ArticleId articleId)
        {
            return String.Format(@"/Content/Images/Articles/{0}", articleId.Id.ToString("D"));
        }

        private ImageStore CreateImageStore(string path)
        {
            return new ImageStore(path);
        }

        private BlueImpFileUploadJsonResultFactory CreateFactory(string path, ArticleId articleId)
        {
            var factory = new BlueImpFileUploadJsonResultFactory();
            factory.ImageUrl = path;
            factory.DeleteUrl = "/api/articles/" + articleId.Id.ToString("D") + "/files";
            return factory;
        }

        [GET("")]
        [Transaction]
        public virtual object Get(ArticleId articleId)
        {
            // TODO: Auth filtering
            var factory = CreateFactory(GetBaseFilesUrl(articleId), articleId);
            var images = _articleFileQueryService.ByArticle(articleId);
            var result = factory.Create(images);
            return new {files = result};
        }

        [POST("")]        
        public virtual object Post(ArticleId articleId)
        {
            var path = GetPath(articleId);
            var store = CreateImageStore(path);
            var factory = CreateFactory(GetBaseFilesUrl(articleId), articleId);
            var handler = new BlueImpFileUploadHandler(store);
            var images = handler.Handle(HttpContext.Current.Request.Files);
            foreach (var each in images)
            {
                var command = new AddImage(CurrentUserId, articleId, Path.GetFileName(each.FileName),
                    each.Type, each.Length);
                Dispatcher.Dispatch(command);
            }
            return new {files = factory.Create(images)};
        }

        [PATCH("{fileName}")]        
        public virtual HttpResponseMessage Patch(ArticleId articleId, string fileName,
            ArticlesFilesPatchRequestContent requestContent)
        {
            Dispatch(new SetPreviewImage(CurrentUserId, articleId, fileName));

            return NoContent();
        }

        [DELETE("{fileName}")]        
        public virtual HttpResponseMessage Delete(ArticleId articleId, string fileName)
        {
            var path = GetPath(articleId);
            var store = CreateImageStore(path);
            Dispatcher.Dispatch(new RemoveImage(CurrentUserId, articleId, fileName));
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