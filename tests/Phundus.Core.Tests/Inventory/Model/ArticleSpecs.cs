namespace Phundus.Tests.Inventory.Model
{
    using System.Linq;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Model;
    using Rhino.Mocks;

    public class article_concern : aggregate_concern_new<Article>
    {
        protected static inventory_factory make;

        protected static Owner theOwner;
        protected static OwnerType theOwnerType;
        protected static StoreId theStoreId;
        protected static ArticleGuid theArticleGuid;
        protected static string theName;
        protected static int theGrossStock;
        protected static decimal thePublicPrice;
        protected static decimal theMemberPrice;

        private Establish ctx = () =>
        {
            make = new inventory_factory(fake);

            theOwnerType = OwnerType.Organization;
            theOwner = null;
            theStoreId = new StoreId();
            theArticleGuid = new ArticleGuid();
            theName = "The name";
            theGrossStock = 10;
            theMemberPrice = 11.11m;
            thePublicPrice = 12.12m;
            sut_factory.create_using(() =>
            {
                theOwner = make.Owner(theOwnerType);
                return new Article(theOwner, theStoreId, theArticleGuid,
                    theName, theGrossStock, thePublicPrice, theMemberPrice);
            });
        };
    }

    [Subject(typeof (Article))]
    public class when_instantiating : article_concern
    {
        private It should_have_the_article_guid = () =>
            sut.ArticleGuid.ShouldEqual(theArticleGuid);

        private It should_have_the_gross_stock = () =>
            sut.GrossStock.ShouldEqual(theGrossStock);

        private It should_have_the_member_price = () =>
            sut.MemberPrice.ShouldEqual(theMemberPrice);

        private It should_have_the_name = () =>
            sut.Name.ShouldEqual(theName);

        private It should_have_the_owner = () =>
            sut.Owner.ShouldEqual(theOwner);

        private It should_have_the_public_price = () =>
            sut.PublicPrice.ShouldEqual(thePublicPrice);

        private It should_have_the_store_id = () =>
            sut.StoreId.ShouldEqual(theStoreId);

        public class when_instanting_for_owner_type_user
        {
            private Establish ctx = () => { theOwnerType = OwnerType.User; };

            private It should_not_have_a_member_price = () =>
                sut.MemberPrice.ShouldBeNull();
        }
    }

    [Subject(typeof (Article))]
    public class when_changing_details : article_concern
    {
        private static string theNewName;
        private static string theNewBrand;
        private static string theNewColor;

        private Establish ctx = () =>
        {
            theNewName = "The new name";
            theNewBrand = "The new brand";
            theNewColor = "The new color";
        };

        private Because of = () =>
            sut.ChangeDetails(theInitiator, theNewName, theNewBrand, theNewColor);

        private It should_have_the_new_brand = () =>
            sut.Brand.ShouldEqual(theNewBrand);

        private It should_have_the_new_color = () =>
            sut.Color.ShouldEqual(theNewColor);

        private It should_have_the_new_name = () =>
            sut.Name.ShouldEqual(theNewName);

        private It should_public_details_changed = () =>
            Published<ArticleDetailsChanged>(p =>
                p.ArticleGuid == theArticleGuid.Id
                && p.Brand == theNewBrand
                && p.Color == theNewColor
                && Equals(p.Initiator, theInitiator)
                && p.Name == theNewName
                && p.OwnerId == theOwner.OwnerId.Id);
    }

    [Subject(typeof (Article))]
    public class when_changing_description : article_concern
    {
        private static string theNewDescription;
        private Establish ctx = () => { theNewDescription = "The new description"; };

        private Because of = () => sut.ChangeDescription(theInitiator, theNewDescription);

        private It should_have_new_description = () =>
            sut.Description.ShouldEqual(theNewDescription);

        private It should_publish_description_changed = () =>
            publisher.WasToldTo(x => x.Publish(
                Arg<DescriptionChanged>.Matches(p => p.ArticleGuid == theArticleGuid.Id
                                                     && p.Description == theNewDescription
                                                     && Equals(p.Initiator, theInitiator)
                                                     && p.OwnerId == theOwner.OwnerId.Id)));
    }

    [Subject(typeof (Article))]
    public class when_changing_specification : article_concern
    {
        private static string theNewSpecification;
        private Establish ctx = () => theNewSpecification = "The new specification";

        private Because of = () => sut.ChangeSpecification(theInitiator, theNewSpecification);

        private It should_have_new_specification = () =>
            sut.Specification.ShouldEqual(theNewSpecification);

        private It should_publish_specification_changed = () =>
            publisher.WasToldTo(x => x.Publish(
                Arg<SpecificationChanged>.Matches(p => p.ArticleGuid == theArticleGuid.Id
                                                       && Equals(p.Initiator, theInitiator)
                                                       && p.OwnerId == theOwner.OwnerId.Id
                                                       && p.Specification == theNewSpecification)));
    }

    [Subject(typeof (Article))]
    public class when_changing_gross_stock : article_concern
    {
        private static int theNewGrossStock;

        private Establish ctx = () =>
            theNewGrossStock = theGrossStock + 1;

        private Because of = () =>
            sut.ChangeGrossStock(theInitiator, theNewGrossStock);

        private It should_have_the_new_gross_stock = () =>
            sut.GrossStock.ShouldEqual(theNewGrossStock);

        private It should_publish_gross_stock_changed = () =>
            Published<GrossStockChanged>(p =>
                p.ArticleGuid == theArticleGuid.Id
                && Equals(p.Initiator, theInitiator)
                && p.NewGrossStock == theNewGrossStock
                && p.OldGrossStock == theGrossStock
                && p.OwnerId == theOwner.OwnerId.Id);
    }

    [Subject(typeof (Article))]
    public class when_changing_prices : article_concern
    {
        private static decimal theNewPublicPrice;
        private static decimal? theNewMemberPrice;

        private Establish ctx = () =>
        {
            theNewPublicPrice = thePublicPrice + 1.00m;
            theNewMemberPrice = theMemberPrice + 1.00m;
        };

        private Because of = () => sut.ChangePrices(theInitiator, theNewPublicPrice, theNewMemberPrice);


        public class for_an_article_with_owner_type_organization
        {
            private Establish ctx = () =>
                theOwnerType = OwnerType.Organization;

            private It should_have_the_new_member_price = () =>
                sut.MemberPrice.ShouldEqual(theNewMemberPrice);

            private It should_have_the_new_public_price = () =>
                sut.PublicPrice.ShouldEqual(theNewPublicPrice);

            private It should_public_prices_changed = () =>
                publisher.WasToldTo(x => x.Publish(Arg<PricesChanged>.Matches(p =>
                    p.ArticleGuid == theArticleGuid.Id
                    && p.Initiator.InitiatorGuid == theInitiatorId.Id
                    && p.MemberPrice == theNewMemberPrice
                    && p.PublicPrice == theNewPublicPrice)));
        }

        public class for_an_article_with_owner_type_user
        {
            private Establish ctx = () =>
                theOwnerType = OwnerType.User;

            private It should_have_the_new_member_price = () =>
                sut.MemberPrice.ShouldBeNull();

            private It should_have_the_new_public_price = () =>
                sut.PublicPrice.ShouldEqual(theNewPublicPrice);

            private It should_public_prices_changed = () =>
                publisher.WasToldTo(x => x.Publish(Arg<PricesChanged>.Matches(p =>
                    p.ArticleGuid == theArticleGuid.Id
                    && p.Initiator.InitiatorGuid == theInitiatorId.Id
                    && p.MemberPrice == null
                    && p.PublicPrice == theNewPublicPrice)));
        }

        public class when_new_member_price_is_null
        {
            private Establish ctx = () =>
            {
                theOwnerType = OwnerType.Organization;
                theNewMemberPrice = null;
            };

            private It should_have_the_new_member_price = () =>
                sut.MemberPrice.ShouldEqual(theNewMemberPrice);

            private It should_have_the_new_public_price = () =>
                sut.PublicPrice.ShouldEqual(theNewPublicPrice);

            private It should_public_prices_changed = () =>
                publisher.WasToldTo(x => x.Publish(Arg<PricesChanged>.Matches(p =>
                    p.ArticleGuid == theArticleGuid.Id
                    && p.Initiator.InitiatorGuid == theInitiatorId.Id
                    && p.MemberPrice == theNewMemberPrice
                    && p.PublicPrice == theNewPublicPrice)));
        }
    }

    [Subject(typeof (Article))]
    public class when_adding_an_image : article_concern
    {
        private Because of = () => sut.AddImage("fileName", "fileType", 123456);

        private It should_add_to_images_collection = () =>
            sut.Images.ShouldNotBeEmpty();

        private It should_publish_image_added = () =>
            publisher.WasToldTo(x => x.Publish(Arg<ImageAdded>.Is.NotNull));

        private It should_set_is_preview = () =>
            sut.Images.First().IsPreview.ShouldBeTrue();
    }

    [Subject(typeof (Article))]
    public class when_adding_a_second_image : article_concern
    {
        private Because of = () =>
        {
            sut.AddImage("first.jpg", "image/jpeg", 1234);
            sut.AddImage("second.jpg", "image/jpeg", 2345);
        };

        private It should_not_set_is_preview = () =>
            sut.Images.Single(p => p.FileName == "second.jpg").IsPreview.ShouldBeFalse();
    }

    [Subject(typeof (Article))]
    public class when_removing_an_image : article_concern
    {
        private Establish ctx = () => sut_setup.run(x =>
        {
            x.AddImage("first.jpg", "image/jpeg", 1234);
            x.AddImage("second.jpg", "image/jpeg", 2345);
            x.AddImage("third.jpg", "image/jpeg", 3456);
        });

        private Because of = () => sut.RemoveImage("first.jpg");

        private It should_publish_image_removed = () =>
            publisher.WasToldTo(x => x.Publish(Arg<ImageRemoved>.Is.NotNull));

        private It should_publish_preview_image_changed = () =>
            publisher.WasToldTo(x => x.Publish(Arg<PreviewImageChanged>.Is.NotNull));

        private It should_remove_from_images_collection = () =>
            sut.Images.ShouldNotContain(p => p.FileName == "first.jpg");

        private It should_set_is_preview_on_other_image = () =>
            sut.Images.SingleOrDefault(p => p.IsPreview).ShouldNotBeNull();
    }

    [Subject(typeof (Article))]
    public class when_setting_preview_image : article_concern
    {
        private Establish ctx = () => sut_setup.run(x =>
        {
            x.AddImage("first.jpg", "image/jpeg", 1234);
            x.AddImage("second.jpg", "image/jpeg", 1234);
        });

        private Because of = () =>
            sut.SetPreviewImage("second.jpg");

        private It should_remove_preview_flag = () =>
            sut.Images.Single(p => p.FileName == "first.jpg").IsPreview.ShouldBeFalse();

        private It should_set_preview_flag = () =>
            sut.Images.Single(p => p.FileName == "second.jpg").IsPreview.ShouldBeTrue();
    }
}