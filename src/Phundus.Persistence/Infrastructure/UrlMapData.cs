namespace Phundus.Persistence.Infrastructure
{
    using FluentNHibernate.Mapping;
    using Web.Projections;

    public class UrlMapData : ClassMap<Web.Projections.UrlMapData>
    {
        public UrlMapData()
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