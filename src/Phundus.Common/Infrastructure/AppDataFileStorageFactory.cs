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
            return CreateStorage("Orders");
        }

        public IFileStore GetOrganizations(Guid organizationId)
        {
            return CreateStorage("Organizations", organizationId);
        }

        public IFileStore GetArticles(Guid articleId)
        {
            return CreateStorage("Articles", articleId);
        }

        private IFileStore CreateStorage(string path)
        {
            return new AppDataFileStore(Path.Combine(_storageDirectory, path));
        }

        private IFileStore CreateStorage(string path, Guid id)
        {
            return new AppDataFileStore(Path.Combine(_storageDirectory, path, id.ToString("D")));
        }
    }
}