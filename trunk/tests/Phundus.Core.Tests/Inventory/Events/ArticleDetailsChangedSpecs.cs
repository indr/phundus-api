namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Model;

    [Subject(typeof (GrossStockChanged))]
    public class GrossStockChangedSpecs : domain_event_concern<GrossStockChanged>
    {
        private static ArticleGuid theArticleId;
        private static OwnerId theOwnerId;
        private static ArticleId theArticleIntegralId;
        private static int theOldGrossStock = 19;
        private static int theNewGrossStock = 20;

        private Establish ctx = () =>
        {
            theArticleIntegralId = new ArticleId(1234);
            theArticleId = new ArticleGuid();
            theOwnerId = new OwnerId();

            sut_factory.create_using(() =>
                new GrossStockChanged(theInitiator, theArticleIntegralId, theArticleId, theOwnerId, theOldGrossStock,
                    theNewGrossStock));
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

        private It should_have_at_5_the_old_gross_stock = () =>
            dataMember(5).ShouldEqual(theOldGrossStock);

        private It should_have_at_6_the_new_gross_stock = () =>
            dataMember(6).ShouldEqual(theNewGrossStock);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Articles.Model.GrossStockChanged");
    }

    [Subject(typeof (ArticleDetailsChanged))]
    public class ArticleDetailsChangedSpecs : domain_event_concern<ArticleDetailsChanged>
    {
        private static ArticleGuid theArticleId;
        private static OwnerId theOwnerId;
        private static ArticleId theArticleIntegralId;
        private static string theName = "The name";
        private static string theBrand = "The brand";
        private static string theColor = "The color";

        private Establish ctx = () =>
        {
            theArticleIntegralId = new ArticleId(1234);
            theArticleId = new ArticleGuid();
            theOwnerId = new OwnerId();

            sut_factory.create_using(() =>
                new ArticleDetailsChanged(theInitiator, theArticleIntegralId, theArticleId, theOwnerId,
                    theName, theBrand, theColor));
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

        private It should_have_at_5_the_name = () =>
            dataMember(5).ShouldEqual(theName);

        private It should_have_at_6_the_brand = () =>
            dataMember(6).ShouldEqual(theBrand);

        private It should_have_at_7_the_color = () =>
            dataMember(7).ShouldEqual(theColor);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Articles.Model.ArticleDetailsChanged");
    }
}