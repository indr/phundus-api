namespace Phundus.Shop.Model.Collaborators
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public interface ICollaboratorService
    {
        Initiator Initiator(InitiatorId initiatorId);
        Manager Manager(LessorId lessorId, UserId userId);
        ICollection<Manager> Managers(LessorId lessorId, bool emailSubscription);
    }
}