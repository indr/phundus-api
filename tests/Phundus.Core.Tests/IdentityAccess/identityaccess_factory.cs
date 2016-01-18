namespace Phundus.Tests.IdentityAccess
{
    using Common.Domain.Model;
    using developwithpassion.specifications.core;
    using developwithpassion.specifications.extensions;
    using Phundus.IdentityAccess.Organizations.Model;

    public class identityaccess_factory : factory_base
    {
        public identityaccess_factory(ICreateFakes fake) : base(fake)
        {
        }

        public Organization Organization()
        {
            var result = fake.an<Organization>();
            result.setup(x => x.Id).Return(new OrganizationId());
            return result;
        }
    }
}