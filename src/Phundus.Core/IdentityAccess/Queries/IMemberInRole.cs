namespace Phundus.IdentityAccess.Queries
{
    using System;
    using Common.Domain.Model;

    public interface IMemberInRole
    {
        void ActiveMember(Guid organizationId, UserId userId);
        void ActiveMember(OwnerId ownerId, UserId userId);
        
        void ActiveChief(Guid organizationId, UserId userId);
        void ActiveManager(OrganizationId organizationId, UserId userId);
        void ActiveChief(OwnerId ownerId, UserId userId);

        bool IsActiveMember(Guid organizationId, UserId userId);
        bool IsActiveMember(OwnerId ownerId, UserId userId);

        bool IsActiveChief(Guid organizationId, UserId userId);
        bool IsActiveChief(OwnerId ownerId, UserId userId);
        Initiator ActiveManager(OrganizationId organizationId, InitiatorId initiatorId);
    }
}