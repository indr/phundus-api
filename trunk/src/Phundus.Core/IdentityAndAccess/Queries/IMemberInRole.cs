﻿namespace Phundus.Core.IdentityAndAccess.Queries
{
    public interface IMemberInRole
    {
        void ActiveMember(int organizationId, int userId);
        void ActiveChief(int organizationId, int userId);

        bool IsActiveMember(int organizationId, int userId);
        bool IsActiveChief(int organizationId, int userId);
    }
}