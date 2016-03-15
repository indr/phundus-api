namespace Phundus.Integration.IdentityAccess
{
    using System;
    using Common.Domain.Model;

    [Obsolete]
    public interface IInitiatorService
    {
        Initiator GetById(InitiatorId initiatorId);
    }
}