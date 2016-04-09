namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Model;

    [Subject(typeof (PricesChanged))]
    public class prices_changed : inventory_domain_event_concern<PricesChanged>
    {
        private static ArticleId theArticleId;
        private static ArticleShortId theArticlShortId;
        private static decimal thePublicPrice;
        private static decimal theMemberPrice;
        private static OwnerId theOwnerId;

        private Establish ctx = () =>
        {
            theArticleId = new ArticleId();
            theArticlShortId = new ArticleShortId(1234);
            theOwnerId = new OwnerId();
            thePublicPrice = 1.11m;
            theMemberPrice = 2.22m;

            sut_factory.create_using(() =>
                new PricesChanged(theManager, theArticlShortId, theArticleId, theOwnerId,
                    thePublicPrice, theMemberPrice));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_initiator = () =>
            dataMember(1).ShouldEqual(theManager.ToActor());

        private It should_have_at_2_the_article_id = () =>
            dataMember(2).ShouldEqual(theArticlShortId.Id);

        private It should_have_at_3_the_article_guid = () =>
            dataMember(3).ShouldEqual(theArticleId.Id);

        private It should_have_at_4_the_public_price = () =>
            dataMember(4).ShouldEqual(thePublicPrice);

        private It should_have_at_5_the_member_price = () =>
            dataMember(5).ShouldEqual(theMemberPrice);

        private It should_have_at_6_the_owner_id = () =>
            dataMember(6).ShouldEqual(theOwnerId.Id);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Articles.Model.PricesChanged");
    }
}