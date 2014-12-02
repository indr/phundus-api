namespace Phundus.Core.Tests.Shop
{
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.Shop.Orders.Model;

    public class OrganizationFactory
    {
        public static Organization Create()
        {
            return Create(1001);
        }

        public static Organization Create(int organizationId)
        {
            return new Organization(organizationId, "Organisation");
        }

        public static Organization Create(OrganizationId organizationId)
        {
            return new Organization(organizationId.Id, "Organisation");
        }
    }
}