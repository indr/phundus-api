namespace Phundus.Rest.FileUpload
{
    using System.Collections.Generic;
    using System.Web;
    using Inventory.Queries;

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
}