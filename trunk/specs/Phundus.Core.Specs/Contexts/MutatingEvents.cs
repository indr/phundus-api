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

        public TDomainEvent GetExpectedEventOnce<TDomainEvent>()
        {
            foreach (var each in _events)
            {
                if (each.GetType() == typeof (TDomainEvent))
                {
                    _events.Remove(each);
                    return (TDomainEvent) each;
                }
            }
            Assert.Fail("No expected event of type " + typeof(TDomainEvent).Name);
            return default(TDomainEvent);
        }        
    }
}