namespace Phundus.Shop.Infrastructure
{
    using Common.Domain.Model;
    using Common.Infrastructure;
    using Model;
    using Model.Orders;

    public class OrderPdfStore : IOrderPdfStore
    {
        private readonly IFileStore _fileStore;
        private readonly IOrderPdfFactory _orderPdfFactory;
        private readonly IOrderRepository _orderRepository;

        public OrderPdfStore(IOrderPdfFactory orderPdfFactory, IFileStoreFactory fileStoreFactory,
            IOrderRepository orderRepository)
        {
            _orderPdfFactory = orderPdfFactory;
            _orderRepository = orderRepository;
            _fileStore = fileStoreFactory.GetOrders();
        }

        public OrderPdf Get(OrderId orderId)
        {
            var order = _orderRepository.GetById(orderId);
            var fileName = orderId.Id.ToString("D") + ".pdf";

            return Get(order, fileName);
        }

        private OrderPdf Get(Order order, string fileName)
        {
            var fileInfo = _fileStore.Get(fileName, order.UnmutatedVersion);
            if (fileInfo == null)
                fileInfo = CreateAndStorePdf(order, fileName);

            return new OrderPdf(order.OrderId, order.OrderShortId, fileInfo.Version, fileInfo.GetStream());
        }

        private StoredFileInfo CreateAndStorePdf(Order order, string fileName)
        {
            var stream = _orderPdfFactory.GeneratePdf(order);

            return _fileStore.Add(fileName, stream, order.UnmutatedVersion, false);
        }
    }
}