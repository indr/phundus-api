namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Model;

    [Subject(typeof (PricesChanged))]
    public class prices_changed : domain_event_concern<PricesChanged>
    {
        private static ArticleGuid theArticleGuid;
        private static int theArticleIntegralId;
        private static decimal thePublicPrice;
        private static decimal theMemberPrice;
        private static OwnerId theOwnerId;

        private Establish ctx = () =>
        {
            theArticleGuid = new ArticleGuid();
            theArticleIntegralId = 1;
            theOwnerId = new OwnerId();
            thePublicPrice = 1.11m;
            theMemberPrice = 2.22m;

            sut_factory.create_using(() =>
                new PricesChanged(theInitiator, theArticleIntegralId, theArticleGuid, theOwnerId,
                    thePublicPrice, theMemberPrice));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_initiator = () =>
            dataMember(1).ShouldEqual(theInitiator);

        private It should_have_at_2_the_article_id = () =>
            dataMember(2).ShouldEqual(theArticleIntegralId);

        private It should_have_at_3_the_article_guid = () =>
            dataMember(3).ShouldEqual(theArticleGuid.Id);

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