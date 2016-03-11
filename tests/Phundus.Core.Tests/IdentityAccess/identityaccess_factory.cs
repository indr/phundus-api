namespace Phundus.Tests.IdentityAccess
{
    using Common.Domain.Model;
    using developwithpassion.specifications.core;
    using developwithpassion.specifications.extensions;
    using Integration.IdentityAccess;
    using Phundus.IdentityAccess.Model.Users;
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
            var account = fake.an<Account>();
            account.setup(x => x.RequestedEmail).Return("requested@test.phundus.ch");

            var result = fake.an<User>();
            result.setup(x => x.UserId).Return(userId);
            result.setup(x => x.FullName).Return("The User");
            result.setup(x => x.EmailAddress).Return("user@test.phundus.ch");
            result.setup(x => x.FirstName).Return("The");
            result.setup(x => x.LastName).Return("User");
            result.setup(x => x.Account).Return(account);
            return result;
        }

        public Manager Manager()
        {
            return Manager(new UserId());
        }

        public Manager Manager(UserId userId)
        {
            return new Manager(userId, "manager@test.phundus.ch", "The Manager");
        }
    }
}