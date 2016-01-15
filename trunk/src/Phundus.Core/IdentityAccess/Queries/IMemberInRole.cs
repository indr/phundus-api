namespace Phundus.IdentityAccess.Queries
{
    using System;
    using Common.Domain.Model;

    public interface IMemberInRole
    {
        void ActiveMember(Guid organizationId, UserGuid userGuid);
        void ActiveMember(OwnerId ownerId, UserGuid userGuid);
        void ActiveMember(Guid organizationId, int userId);
        void ActiveMember(OwnerId ownerId, int userId);
        void ActiveMember(OwnerId ownerId, UserId userId);


        void ActiveChief(Guid organizationId, UserGuid userGuid);
        void ActiveChief(OwnerId ownerId, UserGuid userGuid);
        void ActiveChief(Guid organizationId, int userId);
        void ActiveChief(OwnerId ownerId, int userId);
        void ActiveChief(OwnerId ownerId, UserId userId);


        bool IsActiveMember(Guid organizationId, UserGuid userGuid);
        bool IsActiveMember(OwnerId ownerId, UserGuid userGuid);
        bool IsActiveMember(Guid organizationId, int userId);
        bool IsActiveMember(OwnerId ownerId, int userId);
        bool IsActiveMember(OwnerId ownerId, UserId userId);


        bool IsActiveChief(Guid organizationId, UserGuid userGuid);
        bool IsActiveChief(OwnerId ownerId, UserGuid userId);
        bool IsActiveChief(Guid organizationId, int userId);
        bool IsActiveChief(OwnerId ownerId, int userId);
        bool IsActiveChief(OwnerId ownerId, UserId userId);
    }
}