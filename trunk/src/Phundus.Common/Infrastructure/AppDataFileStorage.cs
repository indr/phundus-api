namespace Phundus.Common.Infrastructure
{
    using System.IO;
    using System.Web.Hosting;

    public interface IFileStorage
    {
        void Store(Storage storage, string fileName, Stream stream);
        Stream Get(Storage storage, string fileName);
    }

    public enum Storage
    {
        Orders
    }

    public class AppDataFileStorage : IFileStorage
    {
        private readonly string _baseDirectory;

        public AppDataFileStorage()
        {
            _baseDirectory = HostingEnvironment.MapPath(@"~\App_Data\Storage\");
        }

        public AppDataFileStorage(string baseDirectory)
        {
            _baseDirectory = baseDirectory;
        }

        public void Store(Storage fileType, string fileName, Stream stream)
        {
            var path = GetPath(fileType, fileName);
            Write(path, stream);
        }

        public Stream Get(Storage fileType, string fileName)
        {
            var path = GetPath(fileType, fileName);
            return Read(path);
        }

        private string GetPath(Storage storage, string fileName)
        {
            var path = Path.Combine(_baseDirectory, storage.ToString());
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return Path.Combine(path, fileName);
        }

        private static void Write(string fullFileName, Stream stream)
        {
            var fileStream = new FileStream(fullFileName, FileMode.Create);
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(fileStream);
            fileStream.Close();
        }

        private static Stream Read(string fullFileName)
        {
            return new FileStream(fullFileName, FileMode.Open);
        }
    }
}