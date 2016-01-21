namespace Phundus.Authorization
{
    using Castle.Transactions;

    public interface IHandleAuthorization
    {
    }

    public interface IHandleAuthorization<in TAccessObject> : IHandleAuthorization
    {
        [Transaction]
        void Handle(TAccessObject accessObject);
    }
}