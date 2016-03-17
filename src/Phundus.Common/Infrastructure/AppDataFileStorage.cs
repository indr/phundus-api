namespace Phundus.Common.Infrastructure
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Hosting;

    public interface IFileStorage
    {
        void Store(Storage storage, string fileName, Stream stream, int version);
        Stream Get(Storage storage, string fileName, int version);
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

        public void Store(Storage fileType, string fileName, Stream stream, int version)
        {
            if (version < 0) throw new ArgumentOutOfRangeException("version");

            var path = GetPath(fileType, fileName, version);
            Write(path, stream);
        }

        public Stream Get(Storage fileType, string fileName, int version)
        {
            if (version <= -1)
                version = FindHighestVersion(fileType, fileName);

            var path = GetPath(fileType, fileName, version);
            return Read(path);
        }

        private int FindHighestVersion(Storage storage, string fileName)
        {
            var storagePath = GetStoragePath(storage);
            var pattern = GetVersionFileName(fileName, "*");
            var regex = new Regex(@"-(\d+)(\.\w+$|$)");

            var versions = Directory.GetFiles(storagePath, pattern)
                .Select(each =>
                {
                    var match = regex.Match(each);
                    if (match.Success)
                        return Convert.ToInt32(match.Groups[1].Value);
                    return (int?)null;
                })
                .ToList();
            if (versions.Count == 0)
                return 0;

            return versions.Max() ?? 0;
        }

        private string GetPath(Storage storage, string fileName, int version)
        {
            var storagePath = GetStoragePath(storage);
            var versionFileName = GetVersionFileName(fileName, version.ToString(CultureInfo.InvariantCulture));
            return Path.Combine(storagePath, versionFileName);
        }

        private string GetStoragePath(Storage storage)
        {
            string storagePath = Path.Combine(_baseDirectory, storage.ToString());
            if (!Directory.Exists(storagePath))
                Directory.CreateDirectory(storagePath);
            return storagePath;
        }

        private string GetVersionFileName(string fileName, string version)
        {
            var extensionIndex = fileName.LastIndexOf(".", System.StringComparison.Ordinal);
            if (extensionIndex == -1)
                return fileName + "-" + version;

            return fileName.Insert(extensionIndex, "-" + version);
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
            if (!File.Exists(fullFileName))
                return null;
            return new FileStream(fullFileName, FileMode.Open);
        }
    }
}