namespace Phundus.Persistence.Inventory.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Phundus.Inventory.AvailabilityAndReservation.Model;
    using Phundus.Shop.Orders.Model;

    public class ReservationFactory
    {
        private Reservation Create(OrderItem orderItem)
        {
            return new Reservation
                       {
                           FromUtc = orderItem.FromUtc,
                           ToUtc = orderItem.ToUtc,
                           Amount = orderItem.Amount,
                           OrderItemId = orderItem.ItemId.Id
                       };
        }

        public IEnumerable<Reservation> Create(IQueryable<OrderItem> query)
        {
            return query.Select(each => Create(each));
        }
    }
}
