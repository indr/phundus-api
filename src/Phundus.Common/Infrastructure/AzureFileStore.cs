namespace Phundus.Common.Infrastructure
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.Azure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.File;

    public class AzureFileStore : IFileStore
    {
        private string _directory;
        private Regex _versionRegex;
        private bool _versioned;

        public string BaseDirectory
        {
            get { return _directory; }
        }

        public AzureFileStore(string directory, bool versioned)
        {
            _directory = directory;
            _versionRegex = new Regex(@"-(\d+)(\.\w+$|$)");
            _versioned = versioned;
        }

        public StoredFileInfo Add(string fileName, Stream stream, int version, bool overwriteExisting)
        {
            var directory = GetDirectory();
            fileName = GetVersionFileName(fileName, version.ToString(CultureInfo.InvariantCulture));
            var file = directory.GetFileReference(fileName);
            file.UploadFromStream(stream);
            return CreateStoredFileInfo(file);
        }

        public StoredFileInfo Get(string fileName, int version)
        {
            var directory = GetDirectory();
            fileName = GetVersionFileName(fileName, version.ToString(CultureInfo.InvariantCulture));
            var file = directory.GetFileReference(fileName);
            return CreateStoredFileInfo(file);
        }

        public StoredFileInfo[] GetFiles()
        {
            var directory = GetDirectory();
            var storedFileInfos = directory.ListFilesAndDirectories()
                .OfType<CloudFile>()
                .Select(s => CreateStoredFileInfo(s));

            var result = storedFileInfos.GroupBy(s => s.Name)
                .Select(g => g.OrderByDescending(s => s.Version).First());

            return result.ToArray();
        }

        public void Remove(string fileName)
        {
            var directory = GetDirectory();
            var file = directory.GetFileReference(fileName);
            if (file.Exists())
            {
                file.Delete();
            }
        }

        private string GetVersionFileName(string fileName, string version)
        {
            if (!_versioned) return fileName;

            var extensionIndex = fileName.LastIndexOf(".", System.StringComparison.Ordinal);
            if (extensionIndex == -1)
                return fileName + "-" + version;

            return fileName.Insert(extensionIndex, "-" + version);
        }

        private CloudFileDirectory GetDirectory()
        {
            var fileShare = GetFileShare();
            var directory = fileShare.GetRootDirectoryReference();
            var parts = _directory.Split('\\');
            foreach (var part in parts)
            {
                directory = directory.GetDirectoryReference(part);
                directory.CreateIfNotExists();
            }
            return directory;
        }

        private static CloudFileShare GetFileShare()
        {
            var shareName = CloudConfigurationManager.GetSetting("StorageShareName");
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var fileClient = storageAccount.CreateCloudFileClient();
            var fileShare = fileClient.GetShareReference(shareName);
            if (!fileShare.Exists())
            {
                throw new Exception(String.Format("File share {0} does not exists", new object[] { shareName }));
            }
            return fileShare;
        }

        private StoredFileInfo CreateStoredFileInfo(CloudFile cloudFile)
        {
            string name = cloudFile.Name;
            int version = 0;
            var match = _versionRegex.Match(cloudFile.Name);
            if (match.Success)
            {
                name = name.Remove(match.Groups[1].Index - 1, match.Groups[1].Length + 1);
                version = Convert.ToInt32(match.Groups[1].Value);
            }

            string extension = cloudFile.Name.Substring(cloudFile.Name.LastIndexOf('.') + 1);
            return new StoredFileInfo(name, version, cloudFile.StorageUri.PrimaryUri.ToString(), extension, cloudFile.Properties.Length, cloudFile.StorageUri.PrimaryUri.ToString());
        }
    }
}