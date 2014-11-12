namespace Phundus.Core.Specs.Contexts
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class PastEvents
    {
        private readonly IList<IDomainEvent> _events = new List<IDomainEvent>();

        public void Add(IDomainEvent e)
        {
            _events.Add(e);
        }

        public IList<IDomainEvent> Events { get { return _events; } }
    }
}