namespace Phundus.Core.Services
{
    public interface IOrganizationService
    {
        void CreateMembershipApplication(int organizationId, int userId);
    }
}