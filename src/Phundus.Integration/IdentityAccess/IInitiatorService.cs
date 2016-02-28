namespace Phundus.Integration.IdentityAccess
{
    using Common.Domain.Model;

    public interface IInitiatorService
    {
        Initiator GetById(InitiatorId initiatorId);
    }
}