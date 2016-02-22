namespace Phundus.Tests.IdentityAccess
{
    using Common.Domain.Model;
    using developwithpassion.specifications.core;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Users.Model;

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

        public Admin Admin()
        {
            return Admin(new UserId());
        }

        public Admin Admin(UserId userId)
        {
            var user = fake.an<Admin>();
            return user;
        }

        public User User()
        {
            return User(new UserId());
        }

        public User User(UserId userId)
        {
            var result = fake.an<User>();
            result.setup(x => x.UserId).Return(userId);
            return result;
        }
    }
}