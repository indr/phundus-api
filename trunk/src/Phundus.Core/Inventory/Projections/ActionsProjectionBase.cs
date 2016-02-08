namespace Phundus.Inventory.Projections
{
    using Common.Domain.Model;
    using Common.Projections;
    using Cqrs;

    public class ActionsProjectionBase<TRow> : ReadModelBase<TRow> where TRow : ActionsProjectionRowBase, new()
    {
        protected virtual TRow CreateRow(DomainEvent @event)
        {
            var row = new TRow();
            row.EventGuid = @event.EventGuid;
            row.Name = @event.GetType().Name;
            row.OccuredOnUtc = @event.OccuredOnUtc;
            return row;
        }
    }
}