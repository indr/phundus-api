namespace Phundus.Core.IdentityAndAccess.Domain.Model.Organizations
{
    using Common.Domain.Model;

    public class OrganizationId : Identity<int>
    {
        public OrganizationId(int id) : base(id)
        {
        }
    }
}