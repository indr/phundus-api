namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using Common.Domain.Model;

    public class ReservationId : Identity
    {
        public ReservationId(string id) : base(id)
        {
        }
    }
}