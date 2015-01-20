namespace Phundus.Core.Inventory.Application.Data
{
    using System;

    public class QuantityAvailableData
    {
        private int _articleId;
        private DateTime _asOfUtc;
        private int _organizationId;
        private string _stockId;

        public QuantityAvailableData(int organizationId, int articleId, string stockId, DateTime asOfUtc)
        {
            _organizationId = organizationId;
            _articleId = articleId;
            _stockId = stockId;
            _asOfUtc = asOfUtc;
        }

        protected QuantityAvailableData()
        {
        }

        public virtual Guid Id { get; protected set; }

        public virtual int ConcurrencyVersion { get; protected set; }

        public virtual int OrganizationId
        {
            get { return _organizationId; }
            protected set { _organizationId = value; }
        }

        public virtual int ArticleId
        {
            get { return _articleId; }
            protected set { _articleId = value; }
        }

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