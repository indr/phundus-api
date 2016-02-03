namespace Phundus.Tests.Inventory.Authorize
{
    using Common;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Authorization;

    [Subject(typeof (ManageArticlesAccessObjectHandler))]
    public class when_handling_manage_articles :
        inventory_access_object_handler_concern<ManageArticlesAccessObject, ManageArticlesAccessObjectHandler>
    {
        private static OwnerId theOwnerId;

        private Establish ctx = () =>
        {
            catchException = true;
            theOwnerId = new OwnerId();
            theAccessObject = new ManageArticlesAccessObject(theOwnerId);
        };

        public class and_the_user_is_a_manager
        {
            private Establish ctx = () =>
                memberInRole.WhenToldTo(x => x.IsActiveManager(theOwnerId, theUserId)).Return(true);

            private It should_have_test_result_true = () =>
                testResult.ShouldBeTrue();

            private It should_not_throw_authoritzation_exception = () =>
                caughtException.ShouldBeNull();
        }

        public class and_the_user_is_not_a_manager
        {
            private Establish ctx = () =>
                memberInRole.WhenToldTo(x => x.IsActiveManager(theOwnerId, theUserId)).Return(false);

            private It should_have_test_result_true = () =>
                testResult.ShouldBeFalse();

            private It should_throw_authoritzation_exception = () =>
                caughtException.ShouldBeOfExactType<AuthorizationException>();

            private It should_throw_exception_message = () =>
                caughtException.Message.ShouldEqual("Du benötigst die Rolle Verwaltung.");
        }
    }
}