namespace Phundus.Authorization
{
    using Common.Domain.Model;

    public interface IHandleAccessObject
    {
    }

    public interface IHandleAccessObject<in TAccessObject> : IHandleAccessObject
    {
        void Enforce(UserId userId, TAccessObject accessObject);
        bool Test(UserId userId, TAccessObject accessObject);
    }
}