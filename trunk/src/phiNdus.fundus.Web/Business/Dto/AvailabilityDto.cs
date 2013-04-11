using System;
using System.ComponentModel.DataAnnotations;

namespace phiNdus.fundus.Business.Dto
{
    public class AvailabilityDto
    {
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Date { get; set; }

        public int Amount { get; set; }
    }
}