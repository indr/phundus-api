namespace phiNdus.fundus.Web.Helpers.FileUpload
{
    using System.IO;
    using System.Web;

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

        public string[] GetFiles()
        {
            Directory.CreateDirectory(MappedFilePath);
            return Directory.GetFiles(MappedFilePath);
        }
    }
}