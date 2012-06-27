using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Domain.Factories
{
    public class ReservationFactory
    {
        private Reservation Create(OrderItem orderItem)
        {
            return new Reservation
                       {
                           From = orderItem.From,
                           To = orderItem.To,
                           Quantity = orderItem.Amount
                       };
        }

        public IEnumerable<Reservation> Create(IQueryable<OrderItem> query)
        {
            return query.Select(each => Create(each));
        }
    }
}
