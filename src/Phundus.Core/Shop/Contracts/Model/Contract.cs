namespace Phundus.Shop.Contracts.Model
{
    using System;
    using Iesi.Collections.Generic;

    public class Contract
    {
        private DateTime _createdOn = DateTime.UtcNow;
        private ISet<ContractItem> _items = new HashedSet<ContractItem>();
        private Lessee _lessee;
        private Guid _organizationId;

        protected Contract()
        {
        }

        public Contract(Guid organizationId, Lessee lessee)
        {
            if (lessee == null)
                throw new ArgumentNullException("lessee");

            _organizationId = organizationId;
            _lessee = lessee;
        }

        public virtual int Id { get; protected set; }

        public virtual int Version { get; protected set; }

        public virtual DateTime CreatedOn
        {
            get { return _createdOn; }
            protected set { _createdOn = value; }
        }

        public virtual DateTime? SignedOn { get; protected set; }

        public virtual Lessee Lessee
        {
            get { return _lessee; }
            protected set { _lessee = value; }
        }

        public virtual ISet<ContractItem> Items
        {
            get { return _items; }
            protected set { _items = value; }
        }

        public virtual int OrderId { get; protected set; }

        public virtual Guid OrganizationId
        {
            get { return _organizationId; }
            protected set { _organizationId = value; }
        }

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