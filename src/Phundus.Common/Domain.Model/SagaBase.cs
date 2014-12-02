namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Cqrs;

    public abstract class SagaBase : ISaga
    {
        private readonly ICollection<IDomainEvent> _uncommittedEvents = new Collection<IDomainEvent>();
        private readonly ICollection<ICommand> _undispatchedCommands = new Collection<ICommand>();

        public Guid Id { get; private set; }

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

        public ICollection<ICommand> UndispatchedCommands
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