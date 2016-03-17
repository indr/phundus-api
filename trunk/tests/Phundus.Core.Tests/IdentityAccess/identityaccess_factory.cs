namespace Phundus.Tests.IdentityAccess
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.core;
    using developwithpassion.specifications.extensions;
    using Phundus.IdentityAccess.Model.Users;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Projections;
    using Phundus.IdentityAccess.Users.Model;

    public class identityaccess_factory : factory_base
    {
        public identityaccess_factory(ICreateFakes fake) : base(fake)
        {
        }

        public Admin Admin(UserId userId = null)
        {
            userId = userId ?? new UserId();
            return new Admin(userId, "admin@test.phundus.ch", "The Admin");
        }

        public Manager Manager(UserId userId = null)
        {
            userId = userId ?? new UserId();
            return new Manager(userId, "manager@test.phundus.ch", "The Manager");
        }

        public Organization Organization(OrganizationId organizationId = null)
        {
            organizationId = organizationId ?? new OrganizationId();
            var result = fake.an<Organization>();
            result.setup(x => x.Id).Return(organizationId);
            return result;
        }

        public OrganizationData OrganizationData()
        {
            var result = fake.an<OrganizationData>();
            result.setup(x => x.OrganizationId).Return(Guid.NewGuid());
            return result;
        }

        public User User(UserId userId = null)
        {
            userId = userId ?? new UserId();

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

        public UserData UserData()
        {
            var result = fake.an<UserData>();
            result.setup(x => x.UserId).Return(Guid.NewGuid());
            result.setup(x => x.EmailAddress).Return("user@test.phundus.ch");
            return result;
        }
    }
}