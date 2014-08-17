namespace Phundus.Core.Inventory.Queries
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AvailabilityDto
    {
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime FromUtc { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime FromLocal { get { return FromUtc.ToLocalTime(); } }

        public int Amount { get; set; }
    }
}