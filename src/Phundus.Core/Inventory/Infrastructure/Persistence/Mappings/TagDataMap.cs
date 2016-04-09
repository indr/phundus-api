namespace Phundus.Inventory.Infrastructure.Persistence.Mappings
{
    using FluentNHibernate.Mapping;
    using Projections;

    public class TagDataMap : ClassMap<TagData>
    {
        public TagDataMap()
        {
            SchemaAction.All();
            Table("Es_Inventory_Tags");

            Id(x => x.Name).GeneratedBy.Assigned();
            Map(x => x.Count);
        }
    }
}