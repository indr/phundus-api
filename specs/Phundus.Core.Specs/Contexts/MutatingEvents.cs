namespace Phundus.Core.Specs.Contexts
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using NUnit.Framework;

    public class MutatingEvents
    {
        private IList<IDomainEvent> _events = new List<IDomainEvent>();
        private int _mutatingEventIdx;

        public IList<IDomainEvent> Events
        {
            get { return _events; }
            set { _events = value; }
        }

        public T GetNextExpectedEvent<T>()
        {
            Assert.That(_events.Count, Is.GreaterThan(_mutatingEventIdx), "Expected more mutating events");

            var domainEvent = _events[_mutatingEventIdx++];
            Assert.That(domainEvent, Is.TypeOf<T>());
            Assert.That(domainEvent, Is.Not.Null);

            return (T) domainEvent;
        }
    }
}