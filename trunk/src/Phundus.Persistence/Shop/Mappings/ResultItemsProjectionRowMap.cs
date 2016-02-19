namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.Shop.Projections;

    public class ResultItemsProjectionRowMap : ClassMap<ResultItemsProjectionRow>
    {
        public ResultItemsProjectionRowMap()
        {
            SchemaAction.All();
            Table("Es_Shop_ResultItems");

            Id(x => x.RowId).GeneratedBy.GuidComb();
            Map(x => x.ItemId, "ArticleGuid").Unique();
            Map(x => x.ItemShortId, "ArticleId").Unique();
            Map(x => x.CreatedAtUtc).CustomType<UtcDateTimeType>();
            Map(x => x.MemberPrice);
            Map(x => x.Name);
            Map(x => x.OwnerGuid);
            Map(x => x.OwnerName);
            Map(x => x.OwnerType);
            Map(x => x.PreviewImageFileName);
            Map(x => x.PublicPrice);
        }
    }
}