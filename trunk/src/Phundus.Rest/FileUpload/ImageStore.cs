namespace Phundus.Rest.FileUpload
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using Common.Infrastructure;

    public class ImageStore : IFileStore
    {
        public ImageStore(string path)
        {
            BaseDirectory = HostingEnvironment.MapPath(path);
        }

        public string BaseDirectory { get; private set; }

        public void Add(string fileName, Stream stream, int version)
        {
            var path = Path.Combine(BaseDirectory, fileName);
            var fileStream = new FileStream(path, FileMode.Create);
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(fileStream);
            fileStream.Close();
        }

        public Stream Get(string fileName, int version)
        {
            throw new NotImplementedException();
        }

        public void Remove(string fileName)
        {
            var mappedFullFileName = BaseDirectory + Path.DirectorySeparatorChar + fileName;
            if (File.Exists(mappedFullFileName))
                File.Delete(mappedFullFileName);

            if (!Directory.Exists(BaseDirectory))
                return;

            if (Directory.GetFiles(BaseDirectory).Length == 0)
                Directory.Delete(BaseDirectory);
        }

        public StoredFileInfo[] GetFiles()
        {
            Directory.CreateDirectory(BaseDirectory);
            return
                Directory.GetFiles(BaseDirectory)
                    .Select(s => new FileInfo(s))
                    .Select(fi => new StoredFileInfo(fi.Name, 0, fi))
                    .ToArray();
        }
    }
}