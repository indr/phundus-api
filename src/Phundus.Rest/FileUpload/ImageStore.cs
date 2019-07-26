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

            if (BaseDirectory != null && !Directory.Exists(BaseDirectory))
                Directory.CreateDirectory(BaseDirectory);
        }

        public string BaseDirectory { get; private set; }

        public StoredFileInfo Add(string fileName, Stream stream, int version, bool overwriteExisting)
        {
            var fileMode = overwriteExisting ? FileMode.Create : FileMode.CreateNew;
            var path = Path.Combine(BaseDirectory, fileName);
            var fileStream = new FileStream(path, fileMode);
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(fileStream);
            fileStream.Close();

            return null;
        }

        public StoredFileInfo Get(string fileName, int version)
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

        public Stream GetStream(StoredFileInfo info)
        {
            return File.OpenRead(info.FullName);
        }
    }
}