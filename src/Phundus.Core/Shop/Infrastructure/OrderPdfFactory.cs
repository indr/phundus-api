namespace Phundus.Shop.Infrastructure
{
    using System;
    using System.IO;
    using System.Linq;
    using Common.Infrastructure;
    using IdentityAccess.Application;
    using Inventory.Application;
    using Model;

    public interface IOrderPdfFactory
    {
        Stream GeneratePdf(Order order);
    }

    public class OrderPdfFactory : IOrderPdfFactory
    {
        private readonly IFileStoreFactory _fileStoreFactory;
        private readonly IOrganizationQueryService _organizationQueryService;
        private readonly IStoresQueryService _storeQueryService;

        public OrderPdfFactory(IStoresQueryService storeQueryService, IOrganizationQueryService organizationQueryService,
            IFileStoreFactory fileStoreFactory)
        {
            _storeQueryService = storeQueryService;
            _organizationQueryService = organizationQueryService;
            _fileStoreFactory = fileStoreFactory;
        }

        public Stream GeneratePdf(Order order)
        {
            var store = GetStore(order);
            var template = GetTemplate(order.Lessor.LessorId.Id);

            return new OrderPdfGenerator(order, store, template).GeneratePdf();            
        }

        private StoredFileInfo GetTemplate(Guid lessorId)
        {
            var result = _organizationQueryService.FindById(lessorId);
            if (result == null || String.IsNullOrWhiteSpace(result.PdfTemplateFileName))
                return null;

            var fileName = result.PdfTemplateFileName;
            var fileStore = _fileStoreFactory.GetOrganizations(lessorId);
            return fileStore.Get(fileName, -1);
        }

        private StoreDetailsData GetStore(Order order)
        {
            var line = order.Lines.FirstOrDefault(p => p.StoreId != null);
            if (line == null)
                return null;

            return _storeQueryService.GetById(line.StoreId.Id);
        }
    }
}