namespace Phundus.Shop.Model.Orders
{
    using System.IO;
    using Common.Domain.Model;

    public interface IOrderPdfStore
    {
        Stream Get(OrderId orderId);
    }
}