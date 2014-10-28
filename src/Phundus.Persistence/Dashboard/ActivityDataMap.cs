namespace Phundus.Persistence.Dashboard
{
    using Core.Dashboard.Application.Data;
    using FluentNHibernate.Mapping;

    public class ActivityDataMap : ClassMap<ActivityData>
    {
        public ActivityDataMap()
        {
            SchemaAction.All();
            Table("Proj_ActivityData");

            Id(x => x.EventGuid).GeneratedBy.Assigned();

            Map(x => x.Name);
            Map(x => x.OccuredOnUtc);
            Map(x => x.Text).Length(int.MaxValue);
        }
    }
}