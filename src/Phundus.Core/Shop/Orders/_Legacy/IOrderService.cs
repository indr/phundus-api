namespace Phundus.Core.Shop.Orders
{
    using System.IO;

    public interface IOrderService
    {
        void Reject(int id);
        void Confirm(int id);

        // TODO: Dto übergeben
        Stream GetPdf(int id);
        Stream GetPdf(int organizationId, int orderId, int currentUserId);
    }
}