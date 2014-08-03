namespace Phundus.Persistence.Inventory.Mappings
{
    using Core.Inventory.Model;
    using FluentNHibernate.Mapping;

    public class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            Id(x => x.Id).GeneratedBy.Native();
            Version(x => x.Version);

            Map(x => x.OrganizationId).Not.Update();
            Map(x => x.CreateDate, "CreateDate").Not.Update();

            Map(x => x.Name, "Name");
            Map(x => x.Brand);
            Map(x => x.Price, "Price");
            Map(x => x.GrossStock, "Stock");
            Map(x => x.Description);
            Map(x => x.Specification);
            Map(x => x.Color);

            HasMany(x => x.Images).AsSet()
                .KeyColumn("ArticleId")
                .Cascade.None();
        }
    }
}