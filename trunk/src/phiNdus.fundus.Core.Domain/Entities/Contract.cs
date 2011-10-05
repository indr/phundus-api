using System;
using Iesi.Collections.Generic;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class Contract : BaseEntity
    {
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

        private DateTime _createDate;
        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public User Borrower { get; set; }

        public ISet<ContractItem> Items
        {
            get {
                return _items;
            }
            set {
                _items = value;
            }
        }

        public bool AddItem(ContractItem item)
        {
            var result = Items.Add(item);
            item.Contract = this;
            return result;
        }

        public bool RemoveItem(ContractItem item)
        {
            var result = Items.Remove(item);
            item.Contract = null;
            return result;
        }
    }
}