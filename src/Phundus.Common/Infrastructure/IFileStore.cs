namespace Phundus.Common.Infrastructure
{
    using System.IO;

    public interface IFileStore
    {
        string BaseDirectory { get; }
        StoredFileInfo Get(string fileName, int version);
        StoredFileInfo[] GetFiles();
        void Remove(string fileName);
        StoredFileInfo Add(string fileName, Stream stream, int version, bool overwriteExisting);
        Stream GetStream(StoredFileInfo info);
    }
}