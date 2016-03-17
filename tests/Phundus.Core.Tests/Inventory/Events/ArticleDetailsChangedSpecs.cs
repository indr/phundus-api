namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Model;

    [Subject(typeof (ArticleDetailsChanged))]
    public class ArticleDetailsChangedSpecs : inventory_domain_event_concern<ArticleDetailsChanged>
    {
        private static ArticleId theArticleId;
        private static OwnerId theOwnerId;
        private static ArticleShortId the_article_short_integral_id;
        private static string theName = "The name";
        private static string theBrand = "The brand";
        private static string theColor = "The color";

        private Establish ctx = () =>
        {
            the_article_short_integral_id = new ArticleShortId(1234);
            theArticleId = new ArticleId();
            theOwnerId = new OwnerId();

            sut_factory.create_using(() =>
                new ArticleDetailsChanged(theManager, the_article_short_integral_id, theArticleId, theOwnerId,
                    theName, theBrand, theColor));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_initiator = () =>
            dataMember(1).ShouldEqual(theManager.ToActor());

        private It should_have_at_2_the_article_id = () =>
            dataMember(2).ShouldEqual(the_article_short_integral_id.Id);

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