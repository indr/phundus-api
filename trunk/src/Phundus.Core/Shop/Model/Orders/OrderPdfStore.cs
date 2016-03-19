namespace Phundus.Shop.Model.Orders
{
    using Common.Domain.Model;

    public interface IOrderPdfStore
    {
        OrderPdf Get(OrderId orderId);        
    }
}