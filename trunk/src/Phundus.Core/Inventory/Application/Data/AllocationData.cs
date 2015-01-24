namespace Phundus.Core.Inventory.Application.Data
{
    using System;
    using Common.Domain.Model;

    public class AllocationData
    {
        private Guid _allocationId;
        private int _articleId;
        private int _organizationId;
        private string _reservationId;
        private string _stockId;

        public AllocationData(Guid allocationId, int organizationId, int articleId, string stockId, string reservationId)
        {
            _allocationId = allocationId;
            _organizationId = organizationId;
            _articleId = articleId;
            _stockId = stockId;
            _reservationId = reservationId;
        }

        protected AllocationData()
        {
        }

        public virtual Guid AllocationId
        {
            get { return _allocationId; }
            protected set { _allocationId = value; }
        }

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

        public virtual string ReservationId
        {
            get { return _reservationId; }
            protected set { _reservationId = value; }
        }

        public virtual DateTime FromUtc { get; set; }
        public virtual DateTime ToUtc { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string AllocationStatus { get; set; }

        public virtual Period Period
        {
            get {return new Period(FromUtc, ToUtc);}
            set
            {
                FromUtc = value.FromUtc;
                ToUtc = value.ToUtc;
            }
        }
    }
}