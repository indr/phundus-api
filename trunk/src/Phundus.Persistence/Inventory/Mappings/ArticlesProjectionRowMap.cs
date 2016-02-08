namespace Phundus.Persistence.Inventory.Mappings
{
    using Common.Domain.Model;
    using Extensions;
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.Inventory.Projections;

    public class ArticlesProjectionRowMap : ClassMap<ArticlesProjectionRow>
    {
        public ArticlesProjectionRowMap()
        {
            SchemaAction.All();

            Table("Es_Inventory_Articles");

            Id(x => x.ArticleId, "ArticleId").GeneratedBy.Assigned();
            Map(x => x.ArticleId, "ArticleShortId");
            Map(x => x.CreatedAtUtc, "CreatedAtUtc").CustomType<UtcDateTimeType>();
            Map(x => x.OwnerGuid, "OwnerGuid");
            Map(x => x.OwnerName, "OwnerName");
            Map(x => x.OwnerType, "OwnerType").CustomType<OwnerType>();
            Map(x => x.StoreId, "StoreId");
            Map(x => x.Name, "Name");
            Map(x => x.Brand, "Brand");
            Map(x => x.Color, "Color");
            Map(x => x.Description, "Description").WithMaxSize();
            Map(x => x.Specification, "Specification").WithMaxSize();
            Map(x => x.PublicPrice, "PublicPrice");
            Map(x => x.MemberPrice, "MemberPrice");
            Map(x => x.GrossStock, "GrossStock");
        }
    }
}