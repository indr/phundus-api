namespace Phundus.Shop.Infrastructure
{
    using System.IO;
    using Common.Domain.Model;
    using Common.Infrastructure;
    using Model.Orders;

    public class OrderPdfStore : IOrderPdfStore
    {
        private readonly IOrderPdfFactory _orderPdfFactory;
        private readonly IFileStore _fileStore;

        public OrderPdfStore(IOrderPdfFactory orderPdfFactory, IFileStoreFactory fileStoreFactory)
        {
            _orderPdfFactory = orderPdfFactory;
            _fileStore = fileStoreFactory.GetOrders();
        }

        public Stream Get(OrderId orderId, int version)
        {
            var fileName = GetFileName(orderId);
            var stream = _fileStore.Get(fileName, version);
            if (stream != null)
                return stream;
            return CreateAndStorePdf(orderId, fileName, version);
        }

        private Stream CreateAndStorePdf(OrderId orderId, string fileName, int version)
        {
            var stream = _orderPdfFactory.GeneratePdf(orderId);
            _fileStore.Add(fileName, stream, version);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        private static string GetFileName(OrderId orderId)
        {
            return orderId.Id.ToString("D") + ".pdf";
        }
    }
}