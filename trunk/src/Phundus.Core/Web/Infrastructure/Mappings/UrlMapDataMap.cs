namespace Phundus.Web.Infrastructure.Mappings
{
    using Application;
    using FluentNHibernate.Mapping;

    public class UrlMapDataMap : ClassMap<UrlMapData>
    {
        public UrlMapDataMap()
        {
            SchemaAction.All();
            Table("Es_Web_FriendlyUrls");

            Id(x => x.RowId).GeneratedBy.GuidComb();
            Map(x => x.Url).Unique();
            Map(x => x.OrganizationId).Nullable();
            Map(x => x.UserId).Nullable();
        }
    }
}