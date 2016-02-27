namespace Phundus.Persistence.Inventory.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Phundus.Inventory.AvailabilityAndReservation.Model;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;

    public class ReservationFactory
    {
        private Reservation Create(OrderLine orderLine)
        {
            return new Reservation
                       {
                           FromUtc = orderLine.Period.FromUtc,
                           ToUtc = orderLine.Period.ToUtc,
                           Amount = orderLine.Quantity,
                           OrderItemId = orderLine.LineId.Id
                       };
        }

        public IEnumerable<Reservation> Create(IQueryable<OrderLine> query)
        {
            return query.Select(each => Create(each));
        }
    }
}
