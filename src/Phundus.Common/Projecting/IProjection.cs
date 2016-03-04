namespace Phundus.Common.Projecting
{
    using Domain.Model;

    public interface IProjection
    {
        bool CanReset { get; }
        void Reset();
        void Handle(DomainEvent e);
    }
}