namespace Phundus.IdentityAccess.Queries
{
    using System;
    using Common.Domain.Model;

    public interface IMemberInRole
    {
        //void ActiveMember(Guid organizationId, UserId userId);
        //void ActiveMember(OwnerId ownerId, UserId userId);
        
        void ActiveManager(Guid organizationId, UserId userId);
        void ActiveManager(OrganizationId organizationId, UserId userId);
        void ActiveManager(OwnerId ownerId, UserId userId);

        bool IsActiveMember(Guid organizationId, UserId userId);
        bool IsActiveMember(OrganizationId organizationId, UserId userId);
        //bool IsActiveMember(OwnerId ownerId, UserId userId);

        //bool IsActiveChief(Guid organizationId, UserId userId);
        bool IsActiveManager(OrganizationId organizationId, UserId userId);
        bool IsActiveManager(OwnerId ownerId, UserId userId);
        //Initiator ActiveManager(OrganizationId organizationId, InitiatorId initiatorId);
    }
}