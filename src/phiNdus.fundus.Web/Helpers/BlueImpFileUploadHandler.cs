using System.Collections.Generic;
using System.IO;
using System.Web;
using phiNdus.fundus.Business.Dto;

namespace phiNdus.fundus.Web.Helpers
{
    public class ImageStore
    {
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                MappedFilePath = HttpContext.Current.Server.MapPath(_filePath);
            }
        }

        private string MappedFilePath { get; set; }

        public string Save(HttpPostedFileBase file)
        {
            if (!Directory.Exists(MappedFilePath))
                Directory.CreateDirectory(MappedFilePath);

            file.SaveAs(MappedFilePath + Path.DirectorySeparatorChar + file.FileName);
            return FilePath + Path.DirectorySeparatorChar + file.FileName;
        }

        public void Delete(string fileName)
        {
            var mappedFullFileName = MappedFilePath + Path.DirectorySeparatorChar + fileName;
            if (File.Exists(mappedFullFileName))
                File.Delete(mappedFullFileName);


            if (Directory.GetFiles(MappedFilePath).Length == 0)
                Directory.Delete(MappedFilePath);
        }
    }

    public class BlueImpFileUploadHandler
    {
        private readonly ImageStore _store;

        public BlueImpFileUploadHandler(ImageStore store)
        {
            _store = store;
        }

        public IList<ImageDto> Post(HttpFileCollectionBase files)
        {
            var result = new List<ImageDto>();
            foreach (string each in files)
            {
                HttpPostedFileBase file = files[each];
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
                           delete_type = "DELETE",
                           delete_url = DeleteUrl + '/' + fileName,
                           thumbnail_url = ImageUrl + '/' + fileName + ".ashx?maxwidth=80&maxheight=80",
                           url = ImageUrl + '/' + fileName,
                           name = fileName,
                           size = length,
                           type = type
                       };
        }


        public BlueImpFileUploadJsonResult Create(ImageDto image)
        {
            string fileName = Path.GetFileName(image.FileName);
            return Create(fileName, image.Length, image.Type);
        }

        public BlueImpFileUploadJsonResult Create(HttpPostedFileBase file)
        {
            return Create(file.FileName, file.ContentLength, file.ContentType);
        }

        public BlueImpFileUploadJsonResult[] Create(IList<ImageDto> images)
        {
            var result = new List<BlueImpFileUploadJsonResult>();
            foreach (ImageDto each in images)
                result.Add(Create(each));
            return result.ToArray();
        }
    }

    public class BlueImpFileUploadJsonResult
    {
        /// <summary>
        /// Dunno?
        /// </summary>
        public string _ { get; set; }

        /// <summary>
        /// URL zum richtigen Bild
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// URL zum Thumbnail
        /// </summary>
        public string thumbnail_url { get; set; }

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
        public string delete_url { get; set; }

        /// <summary>
        /// HttpVerb
        /// </summary>
        public string delete_type { get; set; }
    }
}