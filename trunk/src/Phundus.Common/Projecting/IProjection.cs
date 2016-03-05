namespace Phundus.Common.Projecting
{
    public interface IProjection
    {
        bool CanReset { get; }
        void Reset();
    }
}