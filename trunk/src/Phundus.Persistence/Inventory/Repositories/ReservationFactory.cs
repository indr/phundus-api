namespace Phundus.Persistence.Inventory.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Phundus.Inventory.AvailabilityAndReservation.Model;
    using Phundus.Shop.Model;
    using Phundus.Shop.Projections;

    public class ReservationFactory
    {
        private Reservation Create(OrderLineData orderLine)
        {
            return new Reservation
            {
                FromUtc = orderLine.FromUtc,
                ToUtc = orderLine.ToUtc,
                Quantity = orderLine.Quantity,
                OrderItemId = orderLine.LineId
            };
        }

        public IEnumerable<Reservation> Create(IEnumerable<OrderLineData> orderLines)
        {
            return orderLines.Select(Create);
        }
    }
}