namespace Phundus.Core.Tests.Shop
{
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
    }
}