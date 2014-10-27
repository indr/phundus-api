namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System;
    using Common.Domain.Model;

    public class ReservationId : Identity<string>
    {
        public ReservationId() : base(Guid.NewGuid().ToString())
        {
        }

        public ReservationId(string id) : base(id)
        {
        }
    }
}