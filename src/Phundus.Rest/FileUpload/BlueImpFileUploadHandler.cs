namespace Phundus.Rest.FileUpload
{
    using System.Collections.Generic;
    using System.Web;
    using Inventory.Projections;

    public class BlueImpFileUploadHandler
    {
        private readonly ImageStore _store;

        public BlueImpFileUploadHandler(ImageStore store)
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

                var image = new ImageData();
                image.IsPreview = false;
                image.FileName = _store.Save(file);
                image.Length = file.ContentLength;
                image.Type = file.ContentType;

                result.Add(image);
            }
            return result;
        }
    }
}