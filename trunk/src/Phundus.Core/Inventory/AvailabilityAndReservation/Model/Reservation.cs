namespace Phundus.Core.Inventory.AvailabilityAndReservation.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Reservation
    {
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime FromUtc { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime ToUtc { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime FromLocal { get { return FromUtc.ToLocalTime(); } }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime ToLocal { get { return ToUtc.ToLocalTime(); } }

        public int Quantity { get; set; }
    }
}