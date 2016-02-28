namespace Phundus.Tests.IdentityAccess.Authorize
{
    using Common;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Authorization;

    [Subject(typeof (ManageUsersAccessObjectHandler))]
    public class when_handling_manage_users
        : identityaccess_access_object_handler_concern<ManageUsersAccessObject, ManageUsersAccessObjectHandler>
    {
        private Establish ctx = () =>
        {
            catchException = true;
            theAccessObject = new ManageUsersAccessObject();
        };

        public class and_the_user_is_an_admin
        {
            private Establish ctx = () =>
                userInRole.setup(x => x.IsAdmin(theUserId)).Return(true);

            private It should_have_test_result_true = () =>
                testResult.ShouldBeTrue();

            private It should_not_throw_authorization_exception = () =>
                caughtException.ShouldBeNull();
        }

        public class and_the_user_is_not_an_admin
        {
            private Establish ctx = () =>
                userInRole.setup(x => x.IsAdmin(theUserId)).Return(false);

            private It should_have_test_result_false = () =>
                testResult.ShouldBeFalse();

            private It should_throw_authorization_exception = () =>
                caughtException.ShouldBeOfExactType<AuthorizationException>();

            private It should_throw_exception_message = () =>
                caughtException.Message.ShouldEqual("Du benötigst die Rolle Administration.");
        }
    }
}