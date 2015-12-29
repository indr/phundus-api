namespace Phundus.Rest.Api.Users
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;

    [RoutePrefix("api/users/{userId}/articles/{articleId}/files")]
    public class UsersArticlesFilesController : ApiControllerBase
    {
        private string GetPath(int articleId)
        {
            return String.Format(@"~\Content\Uploads\Articles\{0}", articleId);
        }

        private string GetBaseFilesUrl(int articleId)
        {
            return String.Format(@"/Content/Uploads/Articles/{0}", articleId);
        }

        private ImageStore CreateImageStore(string path)
        {
            return new ImageStore(path);
        }

        private BlueImpFileUploadJsonResultFactory CreateFactory(string path, int userId, int articleId)
        {
            var factory = new BlueImpFileUploadJsonResultFactory();
            factory.ImageUrl = path;
            factory.DeleteUrl = "/api/users/" + userId + "/articles/" + articleId + "/files";
            return factory;
        }

        [GET("")]
        [Transaction]
        [AllowAnonymous]
        public virtual object Get(int userId, int articleId)
        {
            var path = GetPath(articleId);
            var store = CreateImageStore(path);
            var factory = CreateFactory(GetBaseFilesUrl(articleId), userId, articleId);
            var files = store.GetFiles();
            return new {files = factory.Create(files)};
            //return factory.Create(files);
        }        

        [POST("")]
        [Transaction]
        [AllowAnonymous]
        public virtual object Post(int userId, int articleId)
        {
            var path = GetPath(articleId);
            var store = CreateImageStore(path);
            var factory = CreateFactory(GetBaseFilesUrl(articleId), userId, articleId);
            var handler = new BlueImpFileUploadHandler(store);
            var images = handler.Handle(HttpContext.Current.Request.Files);
            return new {files = factory.Create(images)};
        }

        [DELETE("{fileName}")]
        [Transaction]
        [AllowAnonymous]
        public virtual HttpResponseMessage Delete(int userId, int articleId, string fileName)
        {
            var path = GetPath(articleId);
            var store = CreateImageStore(path);
            store.Delete(fileName);
            return Request.CreateResponse(HttpStatusCode.NoContent);
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


            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);

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

    public class ImageDto
    {
        public int Id { get; set; }
        public int Version { get; set; }

        public bool IsPreview { get; set; }
        public long Length { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }

        public string DisplayLength
        {
            get
            {
                string[] sizes = {"B", "KB", "MB", "GB"};
                double len = Length;
                int order = 0;
                while (len >= 1024 && order + 1 < sizes.Length)
                {
                    order++;
                    len = len/1024;
                }

                // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
                // show a single decimal place, and no space.
                return String.Format("{0:0.##} {1}", len, sizes[order]);
            }
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
}