namespace Phundus.Core.Inventory.AvailabilityAndReservation.Model
{
    using System;

    public class Reservation
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int Quantity { get; set; }
    }
}