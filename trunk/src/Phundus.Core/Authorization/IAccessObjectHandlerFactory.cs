namespace Phundus.Authorization
{
    public interface IAccessObjectHandlerFactory
    {
        IHandleAccessObject<TAccessObject> GetHandlerForAccessObject<TAccessObject>(TAccessObject accessObject);

        IHandleAccessObject<TAccessObject>[] GetHandlersForAccessObject<TAccessObject>(TAccessObject accessObject);
    }
}