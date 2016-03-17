namespace Phundus.Persistence.Shop.Projections
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Application;

    public class ShopItemImageDataMap : ClassMap<ShopItemImageData>
    {
        public ShopItemImageDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_Item_Images");

            Id(x => x.DataId).GeneratedBy.GuidComb();

            References(x => x.ShopItem, "ArticleId").UniqueKey("ArticleId_FileName");

            Map(x => x.FileLength);
            Map(x => x.FileName).UniqueKey("ArticleId_FileName");
            Map(x => x.FileType);
        }
    }
}