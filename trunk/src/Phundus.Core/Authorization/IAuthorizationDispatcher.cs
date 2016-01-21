namespace Phundus.Authorization
{
    public interface IAuthorizationDispatcher
    {
        void Dispatch<TAuthorization>(TAuthorization authorization);
    }
}