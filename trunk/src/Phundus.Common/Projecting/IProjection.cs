namespace Phundus.Common.Projecting
{
    using System;
    using Domain.Model;

    public interface IProjection
    {
        void Reset();
        Type GetEntityType();
        void Handle(DomainEvent e);
    }
}