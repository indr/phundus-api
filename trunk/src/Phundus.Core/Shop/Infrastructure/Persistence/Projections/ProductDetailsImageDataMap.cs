namespace Phundus.Shop.Infrastructure.Persistence.Projections
{
    using Application;
    using FluentNHibernate.Mapping;

    public class ProductDetailsImageDataMap : ClassMap<ProductDetailsImageData>
    {
        public ProductDetailsImageDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_ProductDetails_Image");

            Id(x => x.DataId).GeneratedBy.GuidComb();

            References(x => x.ProductDetails, "ArticleId").UniqueKey("ArticleId_FileName");

            Map(x => x.FileLength);
            Map(x => x.FileName).UniqueKey("ArticleId_FileName");
            Map(x => x.FileType);
        }
    }
}