namespace Phundus.Core.Shop.Orders
{
    using System.IO;

    public interface IPdfStore
    {
        // TODO: Dto übergeben
        Stream GetOrderPdf(int orderId);
        Stream GetOrderPdf(int organizationId, int orderId, int currentUserId);
    }
}