﻿namespace Phundus.Core.Shop.Contracts.Model
{
    using System;
    using Ddd;
    using Inventory.Model;
    using Orders.Model;

    public class ContractItem : EntityBase
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