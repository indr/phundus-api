namespace Phundus.Tests.IdentityAccess.Model
{
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Users.Model;
    using Phundus.IdentityAccess.Users.Repositories;
    using Phundus.IdentityAccess.Users.Services;

    public class user_in_role_concern : concern<UserInRole>
    {
        protected static IUserRepository userRepository;

        protected static identityaccess_factory make;

        private Establish ctx = () =>
        {
            make = new identityaccess_factory(fake);
            userRepository = depends.on<IUserRepository>();
        };
    }

    [Subject(typeof (UserInRole))]
    public class when_asking_for_user_with_user_role : user_in_role_concern
    {
        private static User theUser;
        private static UserId theUserId;

        private It admin_should_throw_authorization_exception = () =>
            Catch.Exception(() => sut.Admin(theUserId)).ShouldNotBeNull();

        private Establish ctx = () =>
        {
            theUser = make.User();
            theUserId = theUser.UserId;
            userRepository.WhenToldTo(x => x.GetByGuid(theUser.UserId)).Return(theUser);
        };

        private It is_admin_should_return_false = () =>
            sut.IsAdmin(theUserId).ShouldBeFalse();
    }

    [Subject(typeof (UserInRole))]
    public class when_asking_for_user_with_admin_role : user_in_role_concern
    {
        private static User theUser;
        private static UserId theUserId;

        private It admin_should_throw_authorization_exception = () =>
            Catch.Exception(() => sut.Admin(theUserId)).ShouldBeNull();

        private Establish ctx = () =>
        {
            theUser = make.Admin();
            theUserId = theUser.UserId;
            userRepository.WhenToldTo(x => x.GetByGuid(theUser.UserId)).Return(theUser);
        };

        private It is_admin_should_return_false = () =>
            sut.IsAdmin(theUserId).ShouldBeTrue();
    }
}