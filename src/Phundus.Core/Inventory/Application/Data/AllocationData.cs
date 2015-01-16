namespace Phundus.Core.Inventory.Application.Data
{
    using System;

    public class AllocationData
    {
        private Guid _allocationId;
        private int _articleId;
        private int _organizationId;
        private string _stockId;

        public AllocationData(Guid allocationId, int organizationId, int articleId, string stockId)
        {
            _allocationId = allocationId;
            _organizationId = organizationId;
            _articleId = articleId;
            _stockId = stockId;
        }

        protected AllocationData()
        {
        }

        public virtual Guid AllocationId
        {
            get { return _allocationId; }
            set { _allocationId = value; }
        }

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

        public virtual DateTime FromUtc { get; set; }
        public virtual DateTime ToUtc { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string Status { get; set; }
    }
}