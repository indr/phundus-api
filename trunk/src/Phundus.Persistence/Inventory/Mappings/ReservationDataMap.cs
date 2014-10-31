namespace Phundus.Persistence.Inventory.Mappings
{
    using Core.Inventory.Application.Data;
    using FluentNHibernate.Mapping;

    public class ReservationDataMap : ClassMap<ReservationData>
    {
        public ReservationDataMap()
        {
            Id(x => x.ReservationId).GeneratedBy.Assigned();
            Version(x => x.ConcurrencyVersion);

            Map(x => x.ArticleId);
            Map(x => x.OrganizationId);
            Map(x => x.CreatedUtc);
            Map(x => x.UpdatedUtc);

            Map(x => x.FromUtc);
            Map(x => x.ToUtc);
            Map(x => x.Amount);
        }
    }
}