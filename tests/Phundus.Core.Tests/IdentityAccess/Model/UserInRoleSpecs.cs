namespace Phundus.Tests.IdentityAccess.Model
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Integration.IdentityAccess;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Organizations.Repositories;
    using Phundus.IdentityAccess.Users.Model;
    using Phundus.IdentityAccess.Users.Repositories;
    using Phundus.IdentityAccess.Users.Services;

    public class user_in_role_concern : concern<UserInRole>
    {
        protected static identityaccess_factory make;

        protected static IUserRepository userRepository;
        protected static IMembershipRepository membershipRepository;

        protected static User theUser;

        private Establish ctx = () =>
        {
            make = new identityaccess_factory(fake);
            userRepository = depends.on<IUserRepository>();
            membershipRepository = depends.on<IMembershipRepository>();

            theUser = make.User();
            userRepository.WhenToldTo(x => x.GetByGuid(theUser.UserId)).Return(theUser);
        };
    }

    [Subject(typeof (UserInRole))]
    public class when_asking_for_admin_with_user_in_role_user : user_in_role_concern
    {
        private Establish ctx = () =>
            theUser.WhenToldTo(x => x.Role).Return(UserRole.User);

        private It is_admin_should_return_false = () =>
            sut.IsAdmin(theUser.UserId).ShouldBeFalse();

        private It should_throw_exception = () =>
            Catch.Exception(() => sut.Admin(theUser.UserId)).ShouldNotBeNull();
    }

    [Subject(typeof (UserInRole))]
    public class when_asking_for_admin_with_user_in_admin_role : user_in_role_concern
    {
        private Establish ctx = () =>
            theUser.WhenToldTo(x => x.Role).Return(UserRole.Admin);

        private It is_admin_should_return_true = () =>
            sut.IsAdmin(theUser.UserId).ShouldBeTrue();

        private It should_not_throw_exception = () =>
            Catch.Exception(() => sut.Admin(theUser.UserId)).ShouldBeNull();
    }

    [Subject(typeof (UserInRole))]
    public class when_asking_for_founder : user_in_role_concern
    {
        private static Founder aFounder;

        private Because of = () =>
            aFounder = sut.Founder(theUser.UserId);

        private It should_return_a_founer = () =>
            aFounder.ShouldNotBeNull();
    }

    [Subject(typeof (UserInRole))]
    public class when_asking_for_founder_when_user_is_locked : user_in_role_concern
    {
        private Establish ctx = () => theUser.setup(x => x.IsLocked).Return(true);

        private Because of = () =>
            caughtException = Catch.Exception(() => sut.Founder(theUser.UserId));

        private It should_throw_exception = () =>
            caughtException.ShouldNotBeNull();
    }

    [Subject(typeof (UserInRole))]
    public class when_asking_for_manager_with_user_in_admin_role : user_in_role_concern
    {
        private static Manager aManager;

        private Establish ctx = () =>
            theUser.WhenToldTo(x => x.Role).Return(UserRole.Admin);

        private Because of = () =>
            aManager = sut.Manager(theUser.UserId, new OrganizationId());

        private It should_return_a_manager = () =>
            aManager.ShouldNotBeNull();
    }

    [Subject(typeof (UserInRole))]
    public class when_asking_for_manager_with_user_as_organization_member : user_in_role_concern
    {
        private static OrganizationId theOrganizationId;

        private Establish ctx = () =>
        {
            theUser.WhenToldTo(x => x.Role).Return(UserRole.User);
            theOrganizationId = new OrganizationId();
            var membership = fake.an<Membership>();
            membership.setup(x => x.MemberRole).Return(MemberRole.Member);
            membership.setup(x => x.OrganizationId).Return(theOrganizationId);
            membershipRepository.WhenToldTo(x => x.FindByUserId(theUser.UserId.Id))
                .Return(new List<Membership> {membership});
        };

        private Because of = () =>
            caughtException = Catch.Exception(() => sut.Manager(theUser.UserId, new OrganizationId()));

        private It should_throw_exception = () =>
            caughtException.ShouldNotBeNull();
    }

    [Subject(typeof (UserInRole))]
    public class when_asking_for_manager_with_user_as_organization_manager : user_in_role_concern
    {
        private static OrganizationId theOrganizationId;
        private static Manager aManager;

        private Establish ctx = () =>
        {
            theOrganizationId = new OrganizationId();
            var membership = fake.an<Membership>();
            membership.setup(x => x.MemberRole).Return(MemberRole.Manager);
            membership.setup(x => x.OrganizationId).Return(theOrganizationId);
            membershipRepository.WhenToldTo(x => x.FindByUserId(theUser.UserId.Id))
                .Return(new List<Membership> {membership});
        };

        private Because of = () =>
            aManager = sut.Manager(theUser.UserId, theOrganizationId);

        private It should_return_a_manager = () =>
            aManager.ShouldNotBeNull();
    }

    [Subject(typeof (UserInRole))]
    public class when_asking_for_manager_with_user_as_organization_manager_but_locked : user_in_role_concern
    {
        private static OrganizationId theOrganizationId;

        private Establish ctx = () =>
        {
            theOrganizationId = new OrganizationId();
            var membership = fake.an<Membership>();
            membership.setup(x => x.MemberRole).Return(MemberRole.Manager);
            membership.setup(x => x.OrganizationId).Return(theOrganizationId);
            membership.setup(x => x.IsLocked).Return(true);
            membershipRepository.WhenToldTo(x => x.FindByUserId(theUser.UserId.Id))
                .Return(new List<Membership> {membership});
        };

        private Because of = () =>
            caughtException = Catch.Exception(() => sut.Manager(theUser.UserId, theOrganizationId));

        private It should_throw_exception = () =>
            caughtException.ShouldNotBeNull();
    }
}