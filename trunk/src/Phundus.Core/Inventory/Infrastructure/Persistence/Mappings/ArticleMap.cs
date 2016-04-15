namespace Phundus.Inventory.Infrastructure.Persistence.Mappings
{
    using Articles.Model;
    using FluentNHibernate.Mapping;
    using Model;

    public class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            SchemaAction.Validate();

            Table("Dm_Inventory_Article");
            CompositeId(x => x.ArticleId)
                .KeyProperty(x => x.Id, "ArticleId");
            Component(x => x.ArticleShortId, a =>
                a.Map(x => x.Id, "ArticleShortId")).Not.Update();
            Version(x => x.Version);

            Map(x => x.CreateDate, "CreateDate").Not.Update();

            Component(x => x.Owner, c =>
            {
                c.Component(y => y.OwnerId, d => d.Map(z => z.Id, "Owner_OwnerId"));
                c.Map(y => y.Name, "Owner_Name");
                c.Map(x => x.Type, "Owner_Type").CustomType<OwnerType>();
            });

            Component(x => x.StoreId, c => c.Map(x => x.Id, "StoreId"));

            Map(x => x.Name, "Name");
            Map(x => x.Brand);
            Map(x => x.PublicPrice, "PublicPrice");
            Map(x => x.MemberPrice, "MemberPrice");
            Map(x => x.GrossStock, "Stock");
            Map(x => x.Description);
            Map(x => x.Specification);
            Map(x => x.Color);

            HasMany(x => x.Images).AsSet()
                .KeyColumn("ArticleId").Inverse()
                .Cascade.AllDeleteOrphan();

            Map(x => x.TagsAsString, "Tags");
        }
    }
}