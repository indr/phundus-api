namespace Phundus.Persistence.Inventory.Mappings
{
    using System.Security.Cryptography.X509Certificates;
    using Core.Inventory.Articles.Model;
    using FluentNHibernate.Mapping;

    public class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            SchemaAction.Validate();

            Id(x => x.Id).GeneratedBy.Native();
            Version(x => x.Version);

            Map(x => x.CreateDate, "CreateDate").Not.Update();

            Component(x => x.Owner, c =>
            {
                c.Component(x => x.OwnerId, d => d.Map(x => x.Id, "Owner_OwnerId"));
                c.Map(x => x.Name, "Owner_Name");
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