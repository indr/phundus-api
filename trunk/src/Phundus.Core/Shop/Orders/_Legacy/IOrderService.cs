namespace Phundus.Core.Shop.Orders
{
    using System.IO;
    using Queries;

    public interface IOrderService
    {
        void Reject(int id);
        void Confirm(int id);

        Stream GetPdf(int id);
    }
}