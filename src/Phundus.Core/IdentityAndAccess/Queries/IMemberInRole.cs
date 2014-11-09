namespace Phundus.Core.IdentityAndAccess.Queries
{
    using Domain.Model.Organizations;
    using Domain.Model.Users;

    public interface IMemberInRole
    {
        void ActiveMember(int organizationId, int userId);
        void ActiveMember(OrganizationId organizationId, UserId userId);

        void ActiveChief(int organizationId, int userId);
        void ActiveChief(OrganizationId organizationId, UserId userId);

        bool IsActiveMember(int organizationId, int userId);
        //bool IsActiveChief(int organizationId, int userId);
    }
}