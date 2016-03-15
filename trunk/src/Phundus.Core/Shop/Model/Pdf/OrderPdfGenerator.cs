namespace Phundus.Shop.Model.Pdf
{
    using System.IO;
    using System.Linq;
    using Inventory.Projections;
    using Projections;

    public interface IOrderPdfGenerator
    {
        Stream GeneratePdf(Order order);
    }

    public class OrderPdfGenerator : IOrderPdfGenerator
    {
        private readonly ILessorQueries _lessorQueries;
        private readonly IStoresQueries _storeQueries;

        public OrderPdfGenerator(ILessorQueries lessorQueries, IStoresQueries storeQueries)
        {
            _lessorQueries = lessorQueries;
            _storeQueries = storeQueries;
        }

        public Stream GeneratePdf(Order order)
        {
            var lessor = _lessorQueries.GetById(order.Lessor.LessorId.Id);
            var store = GetStore(order);

            return new OrderPdf(order, lessor, store).GeneratePdf();
        }

        private StoreDetailsData GetStore(Order order)
        {
            var line = order.Lines.FirstOrDefault(p => p.StoreId != null);
            if (line == null)
                return null;

            return _storeQueries.GetById(line.StoreId.Id);
        }
    }
}