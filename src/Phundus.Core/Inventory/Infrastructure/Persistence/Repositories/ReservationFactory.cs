namespace Phundus.Inventory.Infrastructure.Persistence.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using AvailabilityAndReservation.Model;
    using Shop.Application;

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