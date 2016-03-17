namespace Phundus.Shop.Infrastructure.Persistence.Projections
{
    using Application;
    using FluentNHibernate.Mapping;

    public class ShopItemDocumentDataMap : ClassMap<ShopItemDocumentData>
    {
        public ShopItemDocumentDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_Item_Documents");

            Id(x => x.DataId).GeneratedBy.GuidComb();

            References(x => x.ShopItem, "ArticleId").UniqueKey("ArticleId_FileName");

            Map(x => x.FileLength);
            Map(x => x.FileName).UniqueKey("ArticleId_FileName");
            Map(x => x.FileType);
        }
    }
}