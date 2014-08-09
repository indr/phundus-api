namespace Phundus.Core.Shop.Contracts.Model
{
    using System;
    using Iesi.Collections.Generic;

    public class Contract
    {
        private DateTime _createdOn = DateTime.UtcNow;
        private ISet<ContractItem> _items = new HashedSet<ContractItem>();
        private Borrower _borrower;

        protected Contract()
        {
        }

        public Contract(Borrower borrower)
        {
            _borrower = borrower;
        }

        public int Id { get; protected set; }

        public int Version { get; protected set; }

        public virtual DateTime CreatedOn
        {
            get { return _createdOn; }
            protected set { _createdOn = value; }
        }

        public virtual DateTime? SignedOn { get; protected set; }

        public virtual Borrower Borrower
        {
            get { return _borrower; }
            protected set { _borrower = value; }
        }

        public virtual ISet<ContractItem> Items
        {
            get { return _items; }
            protected set { _items = value; }
        }

        public virtual int OrderId { get; protected set; }

        protected virtual bool AddItem(ContractItem item)
        {
            var result = Items.Add(item);
            item.Contract = this;
            return result;
        }

        protected virtual bool RemoveItem(ContractItem item)
        {
            var result = Items.Remove(item);
            item.Contract = null;
            return result;
        }
    }
}