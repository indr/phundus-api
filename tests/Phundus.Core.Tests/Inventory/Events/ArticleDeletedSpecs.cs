namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Model;

    [Subject(typeof (ArticleDeleted))]
    public class ArticleDeletedSpecs : domain_event_concern<ArticleDeleted>
    {
        private static ArticleGuid theArticleId;
        private static OwnerId theOwnerId;
        private static ArticleId theArticleIntegralId;

        private Establish ctx = () =>
        {
            theArticleIntegralId = new ArticleId(1234);
            theArticleId = new ArticleGuid();
            theOwnerId = new OwnerId();

            sut_factory.create_using(() =>
                new ArticleDeleted(theInitiator, theArticleIntegralId, theArticleId, theOwnerId));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_initiator = () =>
            dataMember(1).ShouldEqual(theInitiator);

        private It should_have_at_2_the_article_id = () =>
            dataMember(2).ShouldEqual(theArticleIntegralId.Id);

        private It should_have_at_3_the_article_guid = () =>
            dataMember(3).ShouldEqual(theArticleId.Id);

        private It should_have_at_4_the_owner_guid = () =>
            dataMember(4).ShouldEqual(theOwnerId.Id);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Articles.Model.ArticleDeleted");
    }
}