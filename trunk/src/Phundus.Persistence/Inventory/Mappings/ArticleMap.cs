﻿namespace Phundus.Persistence.Inventory.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Inventory.Articles.Model;

    public class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            SchemaAction.Validate();

            Table("Dm_Inventory_Article");
            Id(x => x.Id).GeneratedBy.Native();
            Component(x => x.ArticleGuid, a =>
                a.Map(x => x.Id)).Not.Update();
            Version(x => x.Version);

            Map(x => x.CreateDate, "CreateDate").Not.Update();

            Component(x => x.Owner, c =>
            {
                c.Component(y => y.OwnerId, d => d.Map(z => z.Id, "Owner_OwnerId"));
                c.Map(y => y.Name, "Owner_Name");
            });

            Component(x => x.StoreId, c => c.Map(x => x.Id, "StoreId"));

            Map(x => x.Name, "Name");
            Map(x => x.Brand);
            Map(x => x.Price, "Price");
            Map(x => x.GrossStock, "Stock");
            Map(x => x.Description);
            Map(x => x.Specification);
            Map(x => x.Color);

            HasMany(x => x.Images).AsSet()
                .KeyColumn("ArticleId").Inverse()
                .Cascade.AllDeleteOrphan();
        }
    }
}