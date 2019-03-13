namespace Phundus.Common.Infrastructure
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Castle.Core.Internal;

    public class AppDataFileStore : IFileStore
    {
        private readonly string _directory;
        private readonly Regex _versionRegex;

        public AppDataFileStore(string directory)
        {
            _directory = directory;
            _versionRegex = new Regex(@"-(\d+)(\.\w+$|$)");
        }

        public string[] GetFileNames()
        {
            var fileNames = Directory.GetFiles(GetStoragePath())
                .Select(each =>
                {
                    var name = new FileInfo(each).Name;
                    var match = _versionRegex.Match(name);
                    if (match.Success)
                        return name.Remove(match.Groups[1].Index - 1, match.Groups[1].Length + 1);
                    return name;
                });

            return fileNames.Distinct().ToArray();
        }

        public StoredFileInfo[] GetFiles()
        {
            var storedFileInfos = Directory.GetFiles(GetStoragePath())
                .Select(s => CreateStoredFileInfo(new FileInfo(s)));

            var result = storedFileInfos.GroupBy(s => s.Name)
                .Select(g => g.OrderByDescending(s => s.Version).First());

            return result.ToArray();
        }

        private StoredFileInfo CreateStoredFileInfo(FileInfo fileInfo)
        {
            string name = fileInfo.Name;
            int version = 0;
            var match = _versionRegex.Match(fileInfo.Name);
            if (match.Success)
            {
                name = name.Remove(match.Groups[1].Index - 1, match.Groups[1].Length + 1);
                version = Convert.ToInt32(match.Groups[1].Value);
            }

            return new StoredFileInfo(name, version, fileInfo);
        }

        public void Remove(string fileName)
        {
            var storagePath = GetStoragePath();
            var pattern = GetVersionFileName(fileName, "*");
            var fileNames = Directory.GetFiles(storagePath, pattern);
            fileNames.ForEach(File.Delete);
        }

        public StoredFileInfo Add(string fileName, Stream stream, int version, bool overwriteExisting = false)
        {
            if (version < 0) throw new ArgumentOutOfRangeException("version");

            var path = GetPath(fileName, version);
            Write(path, stream, overwriteExisting);

            return new StoredFileInfo(fileName, version, new FileInfo(path));
        }

        public string BaseDirectory
        {
            get { return _directory; }
        }

        public StoredFileInfo Get(string fileName, int version)
        {
            if (version <= -1)
                version = FindHighestVersion(fileName);

            var path = GetPath(fileName, version);
            if (!File.Exists(path))
                return null;
            var fileInfo = new FileInfo(path);
            return CreateStoredFileInfo(fileInfo);
        }

        private int FindHighestVersion(string fileName)
        {
            var storagePath = GetStoragePath();
            var pattern = GetVersionFileName(fileName, "*");


            var versions = Directory.GetFiles(storagePath, pattern)
                .Select(each =>
                {
                    var match = _versionRegex.Match(each);
                    if (match.Success)
                        return Convert.ToInt32(match.Groups[1].Value);
                    return (int?)null;
                })
                .ToList();
            if (versions.Count == 0)
                return 0;

            return versions.Max() ?? 0;
        }

        private string GetPath(string fileName, int version)
        {
            var storagePath = GetStoragePath();
            var versionFileName = GetVersionFileName(fileName, version.ToString(CultureInfo.InvariantCulture));
            return Path.Combine(storagePath, versionFileName);
        }

        private string GetStoragePath()
        {
            string storagePath = _directory;
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

        private static void Write(string fullFileName, Stream stream, bool overwriteExisting)
        {
            var fileMode = overwriteExisting ? FileMode.Create : FileMode.CreateNew;
            var fileStream = new FileStream(fullFileName, fileMode);
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(fileStream);
            fileStream.Close();
        }
    }
}