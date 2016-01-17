namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Hosting;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
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
            Dispatcher.Dispatch(new RemoveImage
            {
                ArticleId = articleId,
                ImageFileName = fileName,
                InitiatorId = CurrentUserGuid
            });
            store.Delete(fileName);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        public class BlueImpFileUploadHandler
        {
            private readonly ImageStore _store;

            public BlueImpFileUploadHandler(ImageStore store)
            {
                _store = store;
            }

            public IList<ImageDto> Handle(HttpFileCollection files)
            {
                var result = new List<ImageDto>();
                foreach (string each in files)
                {
                    HttpPostedFile file = files[each];
                    if (file == null || file.ContentLength == 0)
                        continue;

                    var image = new ImageDto();
                    image.IsPreview = false;
                    image.FileName = _store.Save(file);
                    image.Length = file.ContentLength;
                    image.Type = file.ContentType;

                    result.Add(image);
                }
                return result;
            }
        }

        public class BlueImpFileUploadJsonResult
        {
            /// <summary>
            /// URL zum richtigen Bild
            /// </summary>
            public string url { get; set; }

            /// <summary>
            /// URL zum Thumbnail
            /// </summary>
            public string thumbnailUrl { get; set; }

            /// <summary>
            /// Name des Bildes
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// Typ, z.B. "image/jpeg"
            /// </summary>
            public string type { get; set; }

            /// <summary>
            /// Grösse in Bytes
            /// </summary>
            public long size { get; set; }

            /// <summary>
            /// REST-Delete-Url
            /// </summary>
            public string deleteUrl { get; set; }

            /// <summary>
            /// HttpVerb
            /// </summary>
            public string deleteType { get; set; }
        }

        public class BlueImpFileUploadJsonResultFactory
        {
            public string ImageUrl { get; set; }

            public string DeleteUrl { get; set; }

            private BlueImpFileUploadJsonResult Create(string fileName, long length, string type)
            {
                return new BlueImpFileUploadJsonResult
                {
                    deleteType = "DELETE",
                    deleteUrl = DeleteUrl + '/' + fileName,
                    thumbnailUrl = ImageUrl + '/' + fileName + ".ashx?maxwidth=120&maxheight=80",
                    url = ImageUrl + '/' + fileName,
                    name = fileName,
                    size = length,
                    type = type
                };
            }


            public BlueImpFileUploadJsonResult Create(ImageDto image)
            {
                string fileName = System.IO.Path.GetFileName(image.FileName);
                return Create(fileName, image.Length, image.Type);
            }

            public BlueImpFileUploadJsonResult Create(HttpPostedFileBase file)
            {
                return Create(file.FileName, file.ContentLength, file.ContentType);
            }

            public BlueImpFileUploadJsonResult[] Create(IEnumerable<ImageDto> images)
            {
                var result = new List<BlueImpFileUploadJsonResult>();
                foreach (var each in images)
                    result.Add(Create(each));
                return result.ToArray();
            }

            public BlueImpFileUploadJsonResult[] Create(string[] images)
            {
                var result = new List<BlueImpFileUploadJsonResult>();
                foreach (var each in images)
                    result.Add(Create(each));
                return result.ToArray();
            }

            private BlueImpFileUploadJsonResult Create(string fileName)
            {
                var info = new System.IO.FileInfo(fileName);
                var extension = Path.GetExtension(fileName);
                if (extension != null)
                    extension = extension.TrimStart('.');
                return Create(info.Name, info.Length, extension);
            }
        }

        public class ImageStore
        {
            private string _filePath;

            public ImageStore(string path)
            {
                FilePath = path;
            }

            public string FilePath
            {
                get { return _filePath; }
                set
                {
                    _filePath = value;
                    MappedFilePath = HostingEnvironment.MapPath(_filePath);
                }
            }

            private string MappedFilePath { get; set; }

            public string Save(HttpPostedFile file)
            {
                if (!Directory.Exists(MappedFilePath))
                    Directory.CreateDirectory(MappedFilePath);

                var fileName = Path.GetFileNameWithoutExtension(file.FileName).ToFriendlyUrl(false) +
                               Path.GetExtension(file.FileName);
                //var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);

                file.SaveAs(MappedFilePath + Path.DirectorySeparatorChar + fileName);
                return FilePath + Path.DirectorySeparatorChar + fileName;
            }

            public void Delete(string fileName)
            {
                var mappedFullFileName = MappedFilePath + Path.DirectorySeparatorChar + fileName;
                if (File.Exists(mappedFullFileName))
                    File.Delete(mappedFullFileName);

                if (Directory.GetFiles(MappedFilePath).Length == 0)
                    Directory.Delete(MappedFilePath);
            }

            public string[] GetFiles()
            {
                Directory.CreateDirectory(MappedFilePath);
                return Directory.GetFiles(MappedFilePath);
            }
        }
    }
}