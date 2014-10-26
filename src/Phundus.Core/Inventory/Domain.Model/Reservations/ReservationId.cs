namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using Common.Domain.Model;

    public class ReservationId : Identity<string>
    {
        public ReservationId(string id) : base(id)
        {
        }
    }
}