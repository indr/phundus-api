namespace Phundus.Core.Inventory._Legacy.Dtos
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AvailabilityDto
    {
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Date { get; set; }

        public int Amount { get; set; }
    }
}