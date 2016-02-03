namespace Phundus.Persistence.Inventory.Mappings
{
    using Common.Domain.Model;
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.Inventory.Projections;

    public class ArticlesProjectionRowMap : ClassMap<ArticlesProjectionRow>
    {
        public ArticlesProjectionRowMap()
        {
            SchemaAction.Validate();

            Table("Es_Inventory_Articles");

            Id(x => x.RowGuid, "RowGuid").GeneratedBy.GuidComb();

            Map(x => x.ArticleId, "ArticleId");
            Map(x => x.ArticleGuid, "ArticleGuid");
            Map(x => x.CreatedAtUtc, "CreatedAtUtc").CustomType<UtcDateTimeType>();
            Map(x => x.OwnerGuid, "OwnerGuid");
            Map(x => x.OwnerName, "OwnerName");
            Map(x => x.OwnerType, "OwnerType").CustomType<OwnerType>();
            Map(x => x.StoreId, "StoreId");
            Map(x => x.Name, "Name");
            Map(x => x.Brand, "Brand");
            Map(x => x.Color, "Color");
            Map(x => x.Description, "Description");
            Map(x => x.Specification, "Specification");
            Map(x => x.PublicPrice, "PublicPrice");
            Map(x => x.MemberPrice, "MemberPrice");
            Map(x => x.GrossStock, "GrossStock");
        }
    }
}