namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;

    public interface IMemberInRole
    {
        void ActiveMember(int organizationId, int userId);
        [Obsolete]
        void ActiveChief(int organizationId, int userId);
        void ActiveChief(Guid organizationId, int userId);

        [Obsolete]
        bool IsActiveMember(int organizationId, int userId);
        bool IsActiveMember(Guid organizationId, int userId);
        [Obsolete]
        bool IsActiveChief(int organizationId, int userId);
        bool IsActiveChief(Guid organizationId, int userId);
    }
}