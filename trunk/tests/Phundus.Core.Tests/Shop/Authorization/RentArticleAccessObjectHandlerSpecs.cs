namespace Phundus.Tests.Shop.Authorization
{
    using Common;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Authorization;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (RentArticleAccessObjectHandler))]
    public class when_handling_rent_article_access_request :
        shop_access_object_handler_concern<RentArticle, RentArticleAccessObjectHandler>
    {
        private static Article theArticle;
        private static Lessor theLessor;

        private Establish ctx = () =>
        {
            catchException = true;
            theArticle = make.Article();
            theLessor = make.Lessor(theArticle.LessorId);
            lessorService.WhenToldTo(x => x.GetById(theLessor.LessorId)).Return(theLessor);
            theAccessObject = new RentArticle(theArticle);
        };

        public class and_the_user_is_a_member
        {
            private Establish ctx = () =>
                memberInRole.WhenToldTo(x => x.IsActiveMember(theArticle.LessorId, theUserId)).Return(true);

            private It should_have_test_result_true = () =>
                testResult.ShouldBeTrue();

            private It should_not_throw_authorization_exception = () =>
                caughtException.ShouldBeNull();
        }

        public class and_the_user_is_not_a_member_and_the_lessor_does_not_public_rental
        {
            private Establish ctx = () =>
            {
                memberInRole.WhenToldTo(x =>
                    x.IsActiveMember(theArticle.LessorId, theUserId)).Return(false);
                theLessor.setup(x => x.DoesPublicRental).Return(false);
            };

            private It should_have_test_result_true = () =>
                testResult.ShouldBeFalse();

            private It should_throw_authoritzation_exception = () =>
                caughtException.ShouldBeOfExactType<AuthorizationException>();

            private It should_throw_exception_message = () =>
                caughtException.Message.ShouldEqual("Du hast keine Berechtigung um diesen Artikel auszuleihen.");
        }

        public class and_the_user_is_not_a_member_and_the_lessor_does_public_rental
        {
            private Establish ctx = () =>
            {
                memberInRole.WhenToldTo(x =>
                    x.IsActiveMember(theArticle.LessorId, theUserId)).Return(false);
                theLessor.setup(x => x.DoesPublicRental).Return(true);
            };

            private It should_have_test_result_true = () =>
                testResult.ShouldBeTrue();

            private It should_not_throw_authorization_exception = () =>
                caughtException.ShouldBeNull();
        }
    }
}