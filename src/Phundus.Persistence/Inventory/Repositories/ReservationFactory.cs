namespace Phundus.Persistence.Inventory.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Phundus.Inventory.AvailabilityAndReservation.Model;
    using Phundus.Shop.Model;

    public class ReservationFactory
    {
        private Reservation Create(OrderLine orderLine)
        {
            return new Reservation
            {
                FromUtc = orderLine.Period.FromUtc,
                ToUtc = orderLine.Period.ToUtc,
                Quantity = orderLine.Quantity,
                OrderItemId = orderLine.LineId.Id
            };
        }

        public IEnumerable<Reservation> Create(IEnumerable<OrderLine> orderLines)
        {
            return orderLines.Select(Create);
        }
    }
}