namespace Phundus.Authorization
{
    public interface IAuthorizationHandlerFactory
    {
        IHandleAuthorization<TAuthorization> GetHandlerForCommand<TAuthorization>(TAuthorization command);

        IHandleAuthorization<TAuthorization>[] GetHandlersForCommand<TAuthorization>(TAuthorization command);
    }
}