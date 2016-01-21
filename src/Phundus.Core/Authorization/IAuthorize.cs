namespace Phundus.Authorization
{
    public interface IAuthorize
    {
        void Dispatch<TAuthorization>(TAuthorization authorization);
    }
}