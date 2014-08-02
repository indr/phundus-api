namespace phiNdus.fundus.Web.Helpers.FileUpload
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using Phundus.Core.Inventory._Legacy.Dtos;

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

        public BlueImpFileUploadJsonResult[] Create(string[] images)
        {
            var result = new List<BlueImpFileUploadJsonResult>();
            foreach (var each in images)
                result.Add(Create(each));
            return result.ToArray();
        }

        private BlueImpFileUploadJsonResult Create(string fileName)
        {
            var info = new FileInfo(fileName);
            return Create(info.Name, info.Length, Path.GetExtension(fileName));
        }
    }
}