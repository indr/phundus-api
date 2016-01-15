namespace Phundus.IdentityAccess.Queries
{
    using System;
    using Common.Domain.Model;

    public interface IMemberInRole
    {
        void ActiveMember(Guid organizationId, UserGuid userGuid);
        void ActiveMember(OwnerId ownerId, UserGuid userGuid);
        
        void ActiveChief(Guid organizationId, UserGuid userGuid);
        void ActiveChief(OwnerId ownerId, UserGuid userGuid);

        bool IsActiveMember(Guid organizationId, UserGuid userGuid);
        bool IsActiveMember(OwnerId ownerId, UserGuid userGuid);

        bool IsActiveChief(Guid organizationId, UserGuid userGuid);
        bool IsActiveChief(OwnerId ownerId, UserGuid userId);
    }
}