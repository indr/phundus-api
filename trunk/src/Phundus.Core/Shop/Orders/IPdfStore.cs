namespace Phundus.Core.Shop.Orders
{
    using System.IO;

    public interface IPdfStore
    {
        // TODO: Dto übergeben
        Stream GetOrderPdf(int orderId, int currentUserId);
        Stream GetOrderPdf(int orderId, int organizationId, int currentUserId);
    }
}