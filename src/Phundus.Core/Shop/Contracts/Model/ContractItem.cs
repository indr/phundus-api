namespace Phundus.Core.Shop.Contracts.Model
{
    using System;
    using Ddd;
    using Orders.Model;
    using Article = Inventory.Articles.Model.Article;

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
        public virtual OrderItem OrderItem { get; set; }
        public virtual int Amount { get; set; }
        public virtual Article Article { get; set; }
        public virtual string InventoryCode { get; set; }
        public virtual string Name { get; set; }
    }
}