﻿namespace Phundus.Persistence.Shop.Mappings
{
    using Core.Shop.Orders.Model;
    using FluentNHibernate.Mapping;

    public class CartItemMap : ClassMap<CartItem>
    {
        public CartItemMap()
        {
            SchemaAction.Validate();

            Table("CartItem");
            Id(x => x.Id).GeneratedBy.HiLo("100");
            Version(x => x.Version);

            References(x => x.Cart, "CartId");

            Map(x => x.Quantity, "Quantity");
            Map(x => x.From, "[From]");
            Map(x => x.To, "[To]");

            Component(x => x.Article, c =>
            {
                c.Map(x => x.ArticleId, "Article_ArticleId");                
                c.Map(x => x.Caption, "Article_Name");
                c.Map(x => x.Price, "Article_UnitPricePerWeek");
                c.Component(x => x.Owner, c2 =>
                {
                    c2.Component(x => x.OwnerId, c3 => c3.Map(x => x.Id, "Article_Owner_OwnerId"));                    
                    c2.Map(x => x.Name, "Article_Owner_Name");
                });
            });
        }
    }
}