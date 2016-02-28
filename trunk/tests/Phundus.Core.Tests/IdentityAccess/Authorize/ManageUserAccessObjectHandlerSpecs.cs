namespace Phundus.Tests.IdentityAccess.Authorize
{
    using Common;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Authorization;

    [Subject(typeof (ManageUserAccessObjectHandler))]
    public class when_handling_manage_user
        : identityaccess_access_object_handler_concern<ManageUserAccessObject, ManageUserAccessObjectHandler>
    {
        private static UserId theAccessObjectUserId;

        private Establish ctx = () =>
        {
            catchException = true;
            theAccessObjectUserId = new UserId();
            theAccessObject = new ManageUserAccessObject(theAccessObjectUserId);
        };

        public class and_the_initiator_is_an_admin
        {
            private Establish ctx = () =>
                userInRole.setup(x => x.IsAdmin(theUserId)).Return(true);

            private It should_have_test_result_true = () =>
                testResult.ShouldBeTrue();

            private It should_not_throw_authorization_exception = () =>
               caughtException.ShouldBeNull();
        }

        public class and_the_initiator_is_not_the_access_object_user_id
        {
            private Establish ctx = () =>
                theUserId = new UserId();

            private It should_have_test_result_false = () =>
                testResult.ShouldBeFalse();

            private It should_throw_authorization_exception = () =>
                caughtException.ShouldBeOfExactType<AuthorizationException>();
        }

        public class and_the_initiator_is_the_access_object_user_id
        {
            private Establish ctx = () =>
                theUserId = theAccessObjectUserId;

            private It should_have_test_result_true = () =>
                testResult.ShouldBeTrue();

            private It should_not_throw_authorization_exception = () =>
                caughtException.ShouldBeNull();
        }
    }
}