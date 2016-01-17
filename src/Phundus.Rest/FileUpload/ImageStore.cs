namespace Phundus.Rest.FileUpload
{
    using System.IO;
    using System.Web;
    using System.Web.Hosting;
    using Common;

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