namespace Phundus.Core.Inventory.Application.Data
{
    using System;

    public class QuantityInInventoryData
    {
        private int _articleId;
        private int _organizationId;
        private string _stockId;

        public QuantityInInventoryData(int organizationId, int articleId, string stockId)
        {
            _organizationId = organizationId;
            _articleId = articleId;
            _stockId = stockId;
        }

        protected QuantityInInventoryData()
        {
        }

        public virtual Guid Id { get; set; }

        public virtual int ConcurrencyVersion { get; set; }

        public virtual int OrganizationId
        {
            get { return _organizationId; }
            set { _organizationId = value; }
        }

        public virtual int ArticleId
        {
            get { return _articleId; }
            set { _articleId = value; }
        }

        public virtual string StockId
        {
            get { return _stockId; }
            set { _stockId = value; }
        }

        public virtual int Total { get; set; }

        public virtual int Change { get; set; }

        public virtual DateTime AsOfUtc { get; set; }

        public virtual string Comment { get; set; }
    }
}