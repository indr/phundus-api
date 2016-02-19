namespace Phundus.Tests.IdentityAccess.Authorize
{
    using Common;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Authorization;

    [Subject(typeof (ManageOrganizationAccessObjectHandler))]
    public class when_handling_manage_organization :
        identityaccess_access_object_handler_concern<ManageOrganizationAccessObject, ManageOrganizationAccessObjectHandler>
    {
        private static OrganizationId theOrganizationId;

        private Establish ctx = () =>
        {
            catchException = true;
            theOrganizationId = new OrganizationId();
            theAccessObject = new ManageOrganizationAccessObject(theOrganizationId);
        };

        public class and_the_user_is_a_manager
        {
            private Establish ctx = () =>
                memberInRole.WhenToldTo(x => x.IsActiveManager(theOrganizationId, theUserId)).Return(true);

            private It should_have_test_result_true = () =>
                testResult.ShouldBeTrue();

            private It should_not_throw_authorization_exception = () =>
                caughtException.ShouldBeNull();
        }

        public class and_the_user_is_not_a_manager
        {
            private Establish ctx = () =>
                memberInRole.WhenToldTo(x => x.IsActiveManager(theOrganizationId, theUserId)).Return(false);

            private It should_have_test_result_false = () =>
                testResult.ShouldBeFalse();

            private It should_throw_authoritzation_exception = () =>
                caughtException.ShouldBeOfExactType<AuthorizationException>();

            private It should_throw_exception_message = () =>
                caughtException.Message.ShouldEqual("Du benötigst die Rolle Verwaltung.");
        }
    }
}