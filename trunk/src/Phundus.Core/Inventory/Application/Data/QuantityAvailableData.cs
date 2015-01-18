namespace Phundus.Core.Inventory.Application.Data
{
    using System;

    public class QuantityAvailableData
    {
        private string _stockId;
        private DateTime _asOfUtc;

        public QuantityAvailableData(string stockId, DateTime asOfUtc)
        {
            _stockId = stockId;
            _asOfUtc = asOfUtc;
        }

        public virtual Guid Id { get; protected set; }
        public virtual int ConcurrencyVersion { get; protected set; }

        public virtual string StockId
        {
            get { return _stockId; }
            protected set { _stockId = value; }
        }

        public virtual DateTime AsOfUtc
        {
            get { return _asOfUtc; }
            protected set { _asOfUtc = value; }
        }

        public virtual int Quantity { get; set; }
    }
}