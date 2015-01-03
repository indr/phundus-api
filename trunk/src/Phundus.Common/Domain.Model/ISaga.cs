namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using Cqrs;

    public interface ISaga
    {
        Guid Id { get; set; }
        long Version { get; } // for optimistic concurrency

        ICollection<IDomainEvent> UncommittedEvents { get; }

        ICollection<object> UndispatchedCommands { get; }

        void Transition(IDomainEvent e);

        void ClearUncommittedEvents();
        void ClearUndispatchedCommands();
    }
}