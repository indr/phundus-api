using System;

namespace phiNdus.fundus.Domain.Entities
{
    public class Reservation
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int Quantity { get; set; }
    }
}