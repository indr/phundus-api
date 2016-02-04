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

            Id(x => x.ArticleGuid).GeneratedBy.Assigned().UniqueKey("ArticleGuid_FileName");
            Map(x => x.ArticleId);
            Map(x => x.FileLength);
            Map(x => x.FileName).UniqueKey("ArticleGuid_FileName"); ;
            Map(x => x.FileType);
        }
    }
}