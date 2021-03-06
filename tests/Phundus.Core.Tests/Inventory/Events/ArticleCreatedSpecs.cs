﻿namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Model;

    [Subject(typeof (ArticleCreated))]
    public class article_created : inventory_domain_event_concern<ArticleCreated>
    {
        private static Owner theOwner;
        private static ArticleShortId theArticleShortId;
        private static string theName;
        private static int theGrossStock;
        private static decimal theMemberPrice;
        private static decimal thePublicPrice;
        private static ArticleId theArticleId;
        private static StoreId theStoreId;
        private static string theStoreName;

        private Establish ctx = () =>
        {           
            theOwner = new Owner(new OwnerId(), "The Owner", OwnerType.User);
            theStoreId = new StoreId();
            theStoreName = "The store name";
            theArticleShortId = new ArticleShortId(1234);
            theArticleId = new ArticleId();
            theName = "The Article";
            theGrossStock = 2;
            theMemberPrice = 3.33m;
            thePublicPrice = 4.44m;
            sut_factory.create_using(() => new ArticleCreated(theManager, theOwner, theStoreId, theStoreName,
                theArticleShortId, theArticleId, theName, theGrossStock, thePublicPrice, theMemberPrice));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_10_the_store_name = () =>
            dataMember(10).ShouldEqual(theStoreName);

        private It should_have_at_1_initiator = () =>
            dataMember(1).ShouldEqual(theManager.ToActor());

        private It should_have_at_2_owner = () =>
            dataMember(2).ShouldEqual(theOwner);

        private It should_have_at_3_the_store_id = () =>
            dataMember(3).ShouldEqual(theStoreId.Id);

        private It should_have_at_4_the_article_short_id = () =>
            dataMember(4).ShouldEqual(theArticleShortId.Id);

        private It should_have_at_5_the_article_id = () =>
            dataMember(5).ShouldEqual(theArticleId.Id);

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
    }
}