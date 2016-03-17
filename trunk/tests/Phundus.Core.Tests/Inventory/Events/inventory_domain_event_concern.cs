namespace Phundus.Tests.Inventory.Events
{
    using Machine.Specifications;
    using Phundus.Inventory.Model;

    public class inventory_domain_event_concern<TDomainEvent> : domain_event_concern<TDomainEvent>
        where TDomainEvent : class
    {
        protected static inventory_factory make;

        protected static Manager theManager;

        private Establish ctx = () =>
        {
            make = new inventory_factory(fake);

            theManager = make.Manager();
        };
    }
}