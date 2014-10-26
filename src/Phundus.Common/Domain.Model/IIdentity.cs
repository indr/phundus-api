namespace Phundus.Common.Domain.Model
{
    public interface IIdentity<out T>
    {
        T Id { get; }
    }
}