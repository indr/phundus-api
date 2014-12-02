namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using Cqrs;

    public interface ISaga
    {
        Guid Id { get; }
        long Version { get; } // for optimistic concurrency

        ICollection<IDomainEvent> UncommittedEvents { get; }

        ICollection<ICommand> UndispatchedCommands { get; }

        void Transition(IDomainEvent e);

        void ClearUncommittedEvents();
        void ClearUndispatchedCommands();
    }
}