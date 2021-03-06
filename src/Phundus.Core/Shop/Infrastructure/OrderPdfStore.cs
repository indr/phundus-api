﻿namespace Phundus.Shop.Infrastructure
{
    using System;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Infrastructure;
    using Model;
    using Model.Orders;
    using Orders.Model;

    public class OrderPdfStore : IOrderPdfStore,
        ISubscribeTo<OrderPlaced>,
        ISubscribeTo<OrderApproved>,
        ISubscribeTo<OrderClosed>,
        ISubscribeTo<OrderRejected>
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

        public void Handle(OrderApproved e)
        {
            Generate(e.OrderId);
        }

        public void Handle(OrderClosed e)
        {
            Generate(e.OrderId);
        }

        public void Handle(OrderPlaced e)
        {
            Generate(e.OrderId);
        }

        public void Handle(OrderRejected e)
        {
            Generate(e.OrderId);
        }

        private OrderPdf Get(Order order, string fileName)
        {
            var fileInfo = _fileStore.Get(fileName, order.UnmutatedVersion);
            if (fileInfo == null)
                fileInfo = CreateAndStorePdf(order, fileName);

            var stream = _fileStore.GetStream(fileInfo);
            return new OrderPdf(order.OrderId, order.OrderShortId, fileInfo.Version, stream);
        }

        private StoredFileInfo CreateAndStorePdf(Order order, string fileName)
        {
            var stream = _orderPdfFactory.GeneratePdf(order);

            return _fileStore.Add(fileName, stream, order.UnmutatedVersion, false);
        }

        private void Generate(Guid orderId)
        {
            Get(new OrderId(orderId));
        }
    }
}