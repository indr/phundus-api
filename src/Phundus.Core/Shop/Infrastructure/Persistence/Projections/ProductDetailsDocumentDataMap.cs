namespace Phundus.Shop.Infrastructure.Persistence.Projections
{
    using Application;
    using FluentNHibernate.Mapping;

    public class ProductDetailsDocumentDataMap : ClassMap<ProductDetailsDocumentData>
    {
        public ProductDetailsDocumentDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_ProductDetails_Document");

            Id(x => x.DataId).GeneratedBy.GuidComb();

            References(x => x.ProductDetails, "ArticleId").UniqueKey("ArticleId_FileName");

            Map(x => x.FileLength);
            Map(x => x.FileName).UniqueKey("ArticleId_FileName");
            Map(x => x.FileType);
        }
    }
}