namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;

    public interface IMemberInRole
    {
        void ActiveMember(Guid organizationId, int userId);
        void ActiveChief(Guid organizationId, int userId);

        bool IsActiveMember(Guid organizationId, int userId);
        bool IsActiveChief(Guid organizationId, int userId);
    }
}