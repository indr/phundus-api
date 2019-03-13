namespace Phundus.Common.Infrastructure
{
    using System.IO;
    using System.Linq;

    public class StoredFileInfo
    {
        private static readonly string[] ImageExtensions = {"jpg", "jpeg", "gif", "png"};

        public StoredFileInfo(string name, int version, FileInfo fileInfo)
        {
            Name = name;
            Version = version;
            Length = fileInfo.Length;
            FullName = fileInfo.FullName;

            Extension = fileInfo.Extension.TrimStart('.').ToLowerInvariant();
            IsImage = ImageExtensions.Contains(Extension);
            if (IsImage)
                MediaType = "image/" + Extension;
            else if (Extension == "txt")
                MediaType = "text/plain";
            else
                MediaType = "application/" + Extension;
        }

        public StoredFileInfo(string name, int version, string fullName, string extension, long length, string publicUrl)
        {
            Name = name;
            Version = version;
            Length = length;
            FullName = fullName;
            Extension = extension.TrimStart('.').ToLowerInvariant();
            IsImage = ImageExtensions.Contains(Extension);
            if (IsImage)
                MediaType = "image/" + Extension;
            else if (Extension == "txt")
                MediaType = "text/plain";
            else
                MediaType = "application/" + Extension;
            PublicUrl = publicUrl;
        }

        public string Name { get; private set; }
        public int Version { get; private set; }
        public long Length { get; private set; }
        public string FullName { get; private set; }
        public string Extension { get; private set; }

        public bool IsImage { get; private set; }
        public string MediaType { get; private set; }
        public string PublicUrl { get; private set; }

        public Stream GetStream()
        {
            return File.OpenRead(FullName);
        }
    }
}