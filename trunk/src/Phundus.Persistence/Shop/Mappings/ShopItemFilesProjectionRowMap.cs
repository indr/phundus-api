namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Projections;

    public class ShopItemFilesProjectionRowMap : ClassMap<ShopItemFilesProjectionRow>
    {
        public ShopItemFilesProjectionRowMap()
        {
            SchemaAction.All();
            Table("Es_Shop_ItemFiles");

            Id(x => x.RowGuid).GeneratedBy.GuidComb();
            Map(x => x.ArticleGuid).UniqueKey("ArticleGuid_FileName"); ;
            Map(x => x.ArticleId);
            Map(x => x.FileLength);
            Map(x => x.FileName).UniqueKey("ArticleGuid_FileName"); ;
            Map(x => x.FileType);
        }
    }
}