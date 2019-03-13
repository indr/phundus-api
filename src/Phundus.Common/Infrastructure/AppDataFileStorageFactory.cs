namespace Phundus.Common.Infrastructure
{
    using System;
    using System.IO;

    public interface IFileStoreFactory
    {
        IFileStore GetOrders();
        IFileStore GetOrganizations(Guid organizationId);
        IFileStore GetArticles(Guid articleId);
    }

    public class AppDataFileStoreFactory : IFileStoreFactory
    {
        private readonly string _storageDirectory;

        public AppDataFileStoreFactory(IHostingEnvironment hostingEnvironment)
        {
            _storageDirectory = hostingEnvironment.MapPath(@"~\App_Data\Storage");
        }

        public IFileStore GetOrders()
        {
            return CreateStorage("Orders", true);
        }

        public IFileStore GetOrganizations(Guid organizationId)
        {
            return CreateStorage("Organizations", organizationId, true);
        }

        public IFileStore GetArticles(Guid articleId)
        {
            return CreateStorage("Articles", articleId, true);
        }

        private IFileStore CreateStorage(string path, bool versioned)
        {
            // return new AppDataFileStore(Path.Combine(_storageDirectory, path));
            return new AzureFileStore(path, versioned);
        }

        private IFileStore CreateStorage(string path, Guid id, bool versioned)
        {
            // return new AppDataFileStore(Path.Combine(_storageDirectory, path, id.ToString("D")));
            return new AzureFileStore(Path.Combine(path, id.ToString("D")), versioned);
        }
    }
}