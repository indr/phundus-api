namespace Phundus.Shop.Infrastructure
{
    using System.IO;
    using System.Linq;
    using Application;
    using Common.Domain.Model;
    using Inventory.Application;
    using Model;

    public interface IOrderPdfFactory
    {
        Stream GeneratePdf(OrderId orderId);
        Stream GeneratePdf(Order order);
    }

    public class OrderPdfFactory : IOrderPdfFactory
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILessorQueryService _lessorQueryService;
        private readonly IStoresQueryService _storeQueryService;        

        public OrderPdfFactory(IOrderRepository orderRepository, ILessorQueryService lessorQueryService, IStoresQueryService storeQueryService)
        {
            _orderRepository = orderRepository;
            _lessorQueryService = lessorQueryService;
            _storeQueryService = storeQueryService;
        }

        public Stream GeneratePdf(OrderId orderId)
        {
            var order = _orderRepository.GetById(orderId);
            return GeneratePdf(order);
        }

        public Stream GeneratePdf(Order order)
        {
            var lessor = _lessorQueryService.GetById(order.Lessor.LessorId.Id);
            var store = GetStore(order);

            return new OrderPdfGenerator(order, lessor, store).GeneratePdf();
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