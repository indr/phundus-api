namespace Phundus.Core.Shop.Orders
{
    using System.IO;

    public interface IOrderService
    {
        // TODO: Dto übergeben
        Stream GetPdf(int id);
        Stream GetPdf(int organizationId, int orderId, int currentUserId);
    }
}