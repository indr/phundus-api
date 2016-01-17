namespace Phundus.Rest.FileUpload
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using Inventory.Queries;

    public class BlueImpFileUploadJsonResultFactory
    {
        public string ImageUrl { get; set; }

        public string DeleteUrl { get; set; }

        private BlueImpFileUploadJsonResult Create(string fileName, long length, string type, bool isPreview = false)
        {
            return new BlueImpFileUploadJsonResult
            {
                deleteType = "DELETE",
                deleteUrl = DeleteUrl + '/' + fileName,
                thumbnailUrl = ImageUrl + '/' + fileName + ".ashx?maxwidth=120&maxheight=80",
                url = ImageUrl + '/' + fileName,
                name = fileName,
                size = length,
                type = type,
                isPreview = isPreview
            };
        }


        public BlueImpFileUploadJsonResult Create(ImageDto image)
        {
            string fileName = System.IO.Path.GetFileName(image.FileName);
            return Create(fileName, image.Length, image.Type, image.IsPreview);
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
}