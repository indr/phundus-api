namespace Phundus.Authorization
{
    using Castle.Transactions;

    public interface IHandleAuthorization
    {
    }

    public interface IHandleAuthorization<in TAuthorization> : IHandleAuthorization
    {
        [Transaction]
        void Handle(TAuthorization command);
    }
}