namespace Phundus.Shop.Model.Pdf
{
    using System.IO;
    using System.Linq;
    using Application;
    using Inventory.Application;
    using Inventory.Projections;

    public interface IOrderPdfGenerator
    {
        Stream GeneratePdf(Order order);
    }

    public class OrderPdfGenerator : IOrderPdfGenerator
    {
        private readonly ILessorQueryService _lessorQueryService;
        private readonly IStoresQueryService _storeQueryService;

        public OrderPdfGenerator(ILessorQueryService lessorQueryService, IStoresQueryService storeQueryService)
        {
            _lessorQueryService = lessorQueryService;
            _storeQueryService = storeQueryService;
        }

        public Stream GeneratePdf(Order order)
        {
            var lessor = _lessorQueryService.GetById(order.Lessor.LessorId.Id);
            var store = GetStore(order);

            return new OrderPdf(order, lessor, store).GeneratePdf();
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