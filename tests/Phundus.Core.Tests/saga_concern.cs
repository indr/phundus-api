namespace Phundus.Core.Tests
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Machine.Specifications;

    public class saga_concern<TSaga> : concern<TSaga> where TSaga : class, ISaga
    {
        protected static IList<IDomainEvent> pastEvents = new List<IDomainEvent>();
        protected static IDomainEvent domainEvent;

        private Establish ctx = () =>
        {
            pastEvents = new List<IDomainEvent>();
            domainEvent = null;
        };

        public Because of = () =>
        {
            foreach (var each in pastEvents)
                sut.Transition(each);

            sut.ClearUncommittedEvents();
            sut.ClearUndispatchedCommands();

            sut.Transition(domainEvent);
        };
    }
}