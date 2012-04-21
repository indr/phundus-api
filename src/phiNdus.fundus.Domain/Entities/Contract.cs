using System;
using Iesi.Collections.Generic;

namespace phiNdus.fundus.Domain.Entities
{
    public class Contract : Entity
    {
        private DateTime _createDate;
        private ISet<ContractItem> _items;

        public Contract()
            : this(0, 0)
        {
        }

        public Contract(int id, int version) : base(id, version)
        {
            _createDate = DateTime.Now;
            _items = new HashedSet<ContractItem>();
        }

        public virtual DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public virtual User Borrower { get; set; }

        public virtual ISet<ContractItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public virtual DateTime From { get; set; }

        public virtual DateTime To { get; set; }

        public virtual Order Order { get; set; }

        public virtual bool AddItem(ContractItem item)
        {
            var result = Items.Add(item);
            item.Contract = this;
            return result;
        }

        public virtual bool RemoveItem(ContractItem item)
        {
            var result = Items.Remove(item);
            item.Contract = null;
            return result;
        }
    }
}