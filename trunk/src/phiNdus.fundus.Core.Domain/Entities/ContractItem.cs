﻿using System;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class ContractItem : BaseEntity
    {
        public ContractItem() : this(0, 0)
        {
        }

        public ContractItem(int id, int version) : base(id, version)
        {
        }

        public virtual DateTime? ReturnDate { get; set; }

        public virtual Contract Contract { get; set; }
        public OrderItem OrderItem { get; set; }
        public virtual int Amount { get; set; }
        public virtual Article Article { get; set; }
        public virtual string InventoryCode { get; set; }
        public virtual string Name { get; set; }
    }
}