namespace Phundus.Tests.Inventory.Events
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Model;

    [Subject(typeof (ArticleCreated))]
    public class article_created : domain_event_concern<ArticleCreated>
    {
        private static Owner theOwner;
        private static int theArticleIntegralId;
        private static string theName;
        private static int theGrossStock;
        private static decimal theMemberPrice;
        private static decimal thePublicPrice;
        private static Guid theArticleGuid;
        private static StoreId theStoreId;

        private Establish ctx = () =>
        {
            theOwner = new Owner(new OwnerId(), "The Owner", OwnerType.User);
            theStoreId = new StoreId();
            theArticleIntegralId = 1;
            theArticleGuid = Guid.NewGuid();
            theName = "The Article";
            theGrossStock = 2;
            theMemberPrice = 3.33m;
            thePublicPrice = 4.44m;
            sut_factory.create_using(() => new ArticleCreated(theInitiator, theOwner, theStoreId,
                theArticleIntegralId, theArticleGuid, theName, theGrossStock, thePublicPrice, theMemberPrice));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_2_owner = () =>
            dataMember(2).ShouldEqual(theOwner);

        private It should_have_at_4_the_article_id = () =>
            dataMember(4).ShouldEqual(theArticleIntegralId);

        private It should_have_at_5_the_article_guid = () =>
            dataMember(5).ShouldEqual(theArticleGuid);

        private It should_have_at_6_the_name = () =>
            dataMember(6).ShouldEqual(theName);

        private It should_have_at_7_the_gross_stock = () =>
            dataMember(7).ShouldEqual(theGrossStock);

        private It should_have_at_8_the_public_price = () =>
            dataMember(8).ShouldEqual(thePublicPrice);

        private It should_have_at_9_the_member_price = () =>
            dataMember(9).ShouldEqual(theMemberPrice);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Articles.Model.ArticleCreated");

        private It should_have_the_store_id_at_3 = () =>
            dataMember(3).ShouldEqual(theStoreId.Id);

        private It should_haver_at_1_initiator = () =>
            dataMember(1).ShouldEqual(theInitiator);
    }
}