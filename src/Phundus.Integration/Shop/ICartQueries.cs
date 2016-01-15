namespace Phundus.Integration.Shop
{
    using Common.Domain.Model;

    public interface ICartQueries
    {
        ICart FindByUserGuid(UserId userId, UserGuid userGuid);

        ICart FindByUserGuid(InitiatorGuid initiatorGuid, UserGuid userGuid);
    }
}