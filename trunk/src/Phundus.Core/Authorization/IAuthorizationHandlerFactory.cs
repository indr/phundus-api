namespace Phundus.Authorization
{
    public interface IAuthorizationHandlerFactory
    {
        IHandleAuthorization<TAccessObject> GetHandlerForAccessObject<TAccessObject>(TAccessObject command);

        IHandleAuthorization<TAccessObject>[] GetHandlersForAccessObject<TAccessObject>(TAccessObject command);
    }
}