namespace Phundus.Tests.Inventory.Events
{
    using Machine.Specifications;

    public class inventory_domain_event_concern<TDomainEvent> : domain_event_concern<TDomainEvent>
        where TDomainEvent : class
    {
        protected static inventory_factory make;

        private Establish ctx = () => { make = new inventory_factory(fake); };
    }
}