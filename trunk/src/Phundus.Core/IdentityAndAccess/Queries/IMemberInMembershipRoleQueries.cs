namespace Phundus.Core.IdentityAndAccess.Queries
{
    public interface IMemberInMembershipRoleQueries
    {
        bool IsActiveMemberIn(int organizationId, int userId);
        bool IsActiveChiefIn(int organizationId, int userId);
    }
}