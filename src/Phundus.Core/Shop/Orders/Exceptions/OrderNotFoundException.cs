namespace Phundus.Core.Shop.Orders
{
    using System;

    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException() : base("Die Bestellung konnte nicht gefunden werden.")
        {
        }

        public OrderNotFoundException(int id)
            : base(string.Format("Die Bestellung mit der Id {0} konnte nicht gefunden werden.", id))
        {
        }
    }
}