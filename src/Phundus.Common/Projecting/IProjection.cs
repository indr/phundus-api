namespace Phundus.Common.Projecting
{
    using Domain.Model;

    public interface IProjection
    {
        void Reset();
        void Handle(DomainEvent e);
    }
}