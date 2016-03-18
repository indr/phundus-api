namespace Phundus.Rest.FileUpload
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using Common;
    using Common.Infrastructure;
    using Inventory.Application;

    public class BlueImpFileUploadHandler
    {
        private readonly IFileStore _store;

        public BlueImpFileUploadHandler(IFileStore store)
        {
            _store = store;
        }

        public IList<ImageData> Handle(HttpFileCollection files)
        {
            var result = new List<ImageData>();
            foreach (string each in files)
            {
                HttpPostedFile file = files[each];
                if (file == null || file.ContentLength == 0)
                    continue;

                var fileName = GetFileName(file.FileName);
                _store.Add(fileName, file.InputStream, 0);

                var image = new ImageData();
                image.IsPreview = false;
                image.FileName = fileName;
                image.Length = file.ContentLength;
                image.Type = file.ContentType;

                result.Add(image);
            }
            return result;
        }

        private string GetFileName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName).ToFriendlyUrl(false) +
                   Path.GetExtension(fileName);
        }
    }
}