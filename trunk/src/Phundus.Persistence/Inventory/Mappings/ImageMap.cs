﻿namespace Phundus.Persistence.Inventory.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Inventory.Articles.Model;

    public class ImageMap : ClassMap<Image>
    {
        public ImageMap()
        {
            SchemaAction.Validate();

            Table("Dm_Inventory_ArticleFile");
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