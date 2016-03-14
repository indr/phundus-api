namespace Phundus.Shop.Model.Pdf
{
    using System.IO;

    public interface IOrderPdfGenerator
    {
        Stream GeneratePdf(Order order);
    }

    public class OrderPdfGenerator : IOrderPdfGenerator
    {
        public Stream GeneratePdf(Order order)
        {
            return new OrderPdf(order).GeneratePdf();
        }
    }
}