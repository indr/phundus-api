namespace Phundus.Rest.FileUpload
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Common.Infrastructure;
    using Inventory.Application;

    public class BlueImpFileUploadJsonResultFactory
    {
        public string ImageUrl { get; set; }

        public string DeleteUrl { get; set; }

        private static string[] _imageTypes = new[] {"png", "jpg", "gif"};

        private BlueImpFileUploadJsonResult Create(string fileName, long length, string type, bool isPreview = false)
        {
            var isImage = type.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase) ||
                          _imageTypes.Contains(type.ToLowerInvariant());
            var thumbnailUrl = isImage ? ImageUrl + '/' + fileName + "?maxwidth=120&maxheight=80" : null;
            return new BlueImpFileUploadJsonResult
            {
                deleteType = "DELETE",
                deleteUrl = DeleteUrl + '/' + fileName,
                thumbnailUrl = thumbnailUrl,
                url = ImageUrl + '/' + fileName,
                name = fileName,
                size = length,
                type = type,
                isPreview = isPreview
            };
        }


        public BlueImpFileUploadJsonResult Create(ImageData image)
        {
            string fileName = System.IO.Path.GetFileName(image.FileName);            
            return Create(fileName, image.Length, image.Type, image.IsPreview);
        }

        public BlueImpFileUploadJsonResult Create(HttpPostedFileBase file)
        {
            return Create(file.FileName, file.ContentLength, file.ContentType);
        }

        public BlueImpFileUploadJsonResult[] Create(IEnumerable<ImageData> images)
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

        public BlueImpFileUploadJsonResult[] Create(StoredFileInfo[] storedFileInfos)
        {
            var result = new List<BlueImpFileUploadJsonResult>();
            foreach (var each in storedFileInfos)
            {
                result.Add(Create(each));
            }
            return result.ToArray();
        }

        private BlueImpFileUploadJsonResult Create(StoredFileInfo info)
        {
            return Create(info.Name, info.Length, info.Extension, false);
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
}