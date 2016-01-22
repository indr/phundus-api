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
            theOwner = new Owner(new OwnerId(), "The owner", OwnerType.User);
            theStoreId = new StoreId();
            theArticleIntegralId = 1;
            theArticleGuid = Guid.NewGuid();
            theName = "The name";
            theGrossStock = 2;
            theMemberPrice = 3.33m;
            thePublicPrice = 4.44m;
            sut_factory.create_using(() => new ArticleCreated(theInitiator, theOwner, theStoreId,
                theArticleIntegralId, theArticleGuid, theName, theGrossStock, thePublicPrice, theMemberPrice));
        };

        private It shold_have_the_name_at_6 = () =>
            dataMember(6).ShouldEqual(theName);

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Articles.Model.ArticleCreated");

        private It should_have_initiator_at_1 = () =>
            dataMember(1).ShouldEqual(theInitiator);

        private It should_have_owner_at_2 = () =>
            dataMember(2).ShouldEqual(theOwner);

        private It should_have_the_store_id_at_3 = () =>
            dataMember(3).ShouldEqual(theStoreId.Id);

        private It should_have_the_article_guid_at_5 = () =>
            dataMember(5).ShouldEqual(theArticleGuid);

        private It should_have_the_article_id_at_4 = () =>
            dataMember(4).ShouldEqual(theArticleIntegralId);

        private It should_have_the_gross_stock_at_7 = () =>
            dataMember(7).ShouldEqual(theGrossStock);

        private It should_have_the_member_price_at_9 = () =>
            dataMember(9).ShouldEqual(theMemberPrice);

        private It should_have_the_public_price_at_8 = () =>
            dataMember(8).ShouldEqual(thePublicPrice);
    }
}