namespace Phundus.Core.IdentityAndAccess.Domain.Model.Organizations
{
    using Common;
    using Common.Domain.Model;

    public class OrganizationId : Identity<int>
    {
        public OrganizationId(int id) : base(id)
        {
            AssertionConcern.AssertArgumentGreaterThan(id, 0, "Organization id must be greater than zero.");
        }
    }
}