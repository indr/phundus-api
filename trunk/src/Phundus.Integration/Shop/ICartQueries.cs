namespace Phundus.Integration.Shop
{
    using Common.Domain.Model;

    public interface ICartQueries
    {
        ICart FindByUserGuid(InitiatorGuid initiatorGuid, UserGuid userGuid);
    }
}