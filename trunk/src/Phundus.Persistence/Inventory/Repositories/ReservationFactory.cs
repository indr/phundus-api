namespace Phundus.Persistence.Inventory.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Inventory.AvailabilityAndReservation.Model;
    using Core.Shop.Orders.Model;

    public class ReservationFactory
    {
        private Reservation Create(OrderItem orderItem)
        {
            return new Reservation
                       {
                           FromUtc = orderItem.FromUtc,
                           ToUtc = orderItem.ToUtc,
                           Quantity = orderItem.Amount
                       };
        }

        public IEnumerable<Reservation> Create(IQueryable<OrderItem> query)
        {
            return query.Select(each => Create(each));
        }
    }
}
