namespace Phundus.Dashboard.Projections
{
    using Common.Projections;

    public class EventLogProjectionRow : ActionsProjectionRowBase
    {
        public virtual string Text { get; set; }
    }
}