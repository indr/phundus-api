namespace Phundus.Authorization
{
    using Common.Domain.Model;

    public interface IHandleAccessObject
    {
    }

    public interface IHandleAccessObject<in TAccessObject> : IHandleAccessObject
    {
        void Handle(UserId userId, TAccessObject accessObject);
    }
}