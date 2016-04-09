namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Model.Articles;

    [Subject(typeof(ProductUntagged))]
    public class ProductUntaggedSpecs : inventory_domain_event_concern<ProductUntagged>
    {
        private static ArticleId theArticleId = new ArticleId();
        private static OwnerId theOwnerId = new OwnerId();

        private Establish ctx = () =>
            sut_factory.create_using(() =>
                new ProductUntagged(theManager, theArticleId, theOwnerId, "tagName"));

        private It should_have_at_1_the_actor = () =>
            dataMember(1).ShouldEqual(theManager.ToActor());

        private It should_have_at_2_the_article_id = () =>
            dataMember(2).ShouldEqual(theArticleId.Id);

        private It should_have_at_3_the_owner_id = () =>
            dataMember(3).ShouldEqual(theOwnerId.Id);

        private It should_have_at_4_the_tag_name = () =>
            dataMember(4).ShouldEqual("tagName");
    }
}