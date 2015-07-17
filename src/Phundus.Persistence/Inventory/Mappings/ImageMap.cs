namespace Phundus.Persistence.Inventory.Mappings
{
    using Core.Inventory.Articles.Model;
    using FluentNHibernate.Mapping;

    public class ImageMap : ClassMap<Image>
    {
        public ImageMap()
        {
            SchemaAction.Validate();

            Id(x => x.Id).GeneratedBy.HiLo("100");
            Version(x => x.Version);

            Map(x => x.IsPreview);
            Map(x => x.Length);
            Map(x => x.Type);
            Map(x => x.FileName);

            References(x => x.Article, "ArticleId");
        }
    }
}