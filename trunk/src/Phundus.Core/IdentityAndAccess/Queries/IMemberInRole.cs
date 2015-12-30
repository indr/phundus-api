namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using Common.Domain.Model;

    public interface IMemberInRole
    {
        void ActiveMember(Guid organizationId, int userId);
        void ActiveChief(Guid organizationId, int userId);
        void ActiveChief(OwnerId ownerId, int userId);
        void ActiveChief(OwnerId ownerId, UserId userId);

        bool IsActiveMember(Guid organizationId, int userId);
        bool IsActiveChief(Guid organizationId, int userId);
        bool IsActiveChief(OwnerId ownerId, int userId);
        void IsActiveChief(OwnerId ownerId, UserId userId);
    }
}