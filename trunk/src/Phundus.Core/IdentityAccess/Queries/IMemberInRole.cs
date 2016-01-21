namespace Phundus.IdentityAccess.Queries
{
    using System;
    using Common.Domain.Model;

    public interface IMemberInRole
    {
        void ActiveManager(OrganizationId organizationId, UserId userId);
        void ActiveManager(OwnerId ownerId, UserId userId);
        void ActiveManager(Guid organizationId, UserId userId);

        bool IsActiveManager(OrganizationId organizationId, UserId userId);
        bool IsActiveManager(OwnerId ownerId, UserId userId);

        bool IsActiveMember(OrganizationId organizationId, UserId userId);
        bool IsActiveMember(LessorId lessorId, UserId userId);
    }
}