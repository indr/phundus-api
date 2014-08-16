namespace Phundus.Core.ReservationCtx.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using Shop.Orders.Model;

    public class ReservationFactory
    {
        private Reservation Create(OrderItem orderItem)
        {
            return new Reservation
                       {
                           From = orderItem.FromUtc.ToLocalTime(),
                           To = orderItem.ToUtc.ToLocalTime(),
                           Quantity = orderItem.Amount
                       };
        }

        public IEnumerable<Reservation> Create(IQueryable<OrderItem> query)
        {
            return query.Select(each => Create(each));
        }
    }
}
