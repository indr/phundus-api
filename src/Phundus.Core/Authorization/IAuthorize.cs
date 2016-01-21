namespace Phundus.Authorization
{
    using Common.Domain.Model;

    public interface IAuthorize
    {
        void User<TAccessObject>(UserId userId, TAccessObject accessObject);
    }
}