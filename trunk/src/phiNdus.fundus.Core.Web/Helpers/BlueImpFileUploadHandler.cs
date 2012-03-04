using System.Collections.Generic;
using System.IO;
using System.Web;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.Helpers
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
                           //thumbnail_url = @"data:image/png;base64,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",
                           thumbnail_url = ImageUrl + '/' + fileName + "/80",
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