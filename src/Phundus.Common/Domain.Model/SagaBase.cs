namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public abstract class SagaBase : ISaga
    {
        private readonly ICollection<IDomainEvent> _uncommittedEvents = new Collection<IDomainEvent>();
        private readonly ICollection<object> _undispatchedCommands = new Collection<object>();
        private Guid _id = Guid.NewGuid();

        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public long Version { get; private set; }

        public void Transition(IDomainEvent e)
        {
            When(e);
            UncommittedEvents.Add(e);
        }

        public ICollection<IDomainEvent> UncommittedEvents
        {
            get { return _uncommittedEvents; }
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        public ICollection<object> UndispatchedCommands
        {
            get { return _undispatchedCommands; }
        }

        public void ClearUndispatchedCommands()
        {
            _undispatchedCommands.Clear();
        }

        protected abstract void When(IDomainEvent e);
    }
}