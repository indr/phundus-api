namespace Phundus.Tests.Inventory.Model
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Model.Articles;
    using Rhino.Mocks;

    public abstract class article_concern : aggregate_concern<Article>
    {
        protected static inventory_factory make;

        protected static Manager theManager;

        protected static Owner theOwner;
        protected static OwnerType theOwnerType;
        protected static StoreId theStoreId;
        protected static ArticleId theArticleId;
        protected static ArticleShortId theArticleShortId;
        protected static string theName;
        protected static int theGrossStock;
        protected static decimal thePublicPrice;
        protected static decimal theMemberPrice;

        private Establish ctx = () =>
        {
            make = new inventory_factory(fake);

            theManager = make.Manager(theInitiatorId);
            theOwnerType = OwnerType.Organization;
            theOwner = null;
            theStoreId = new StoreId();
            theArticleId = new ArticleId();
            theArticleShortId = new ArticleShortId(1234);
            theName = "The name";
            theGrossStock = 10;
            theMemberPrice = 11.11m;
            thePublicPrice = 12.12m;
            sut_factory.create_using(() =>
            {
                theOwner = make.Owner(theOwnerType);
                return new Article(theOwner, theStoreId, theArticleId, theArticleShortId,
                    theName, theGrossStock, thePublicPrice, theMemberPrice);
            });
        };
    }

    [Subject(typeof (Article))]
    public class when_instantiating : article_concern
    {
        private It should_have_no_tags = () =>
            sut.Tags.ShouldBeEmpty();

        private It should_have_the_article_id = () =>
            sut.ArticleId.ShouldEqual(theArticleId);

        private It should_have_the_article_short_id = () =>
            sut.ArticleShortId.ShouldEqual(theArticleShortId);

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
            sut.ChangeDetails(theManager, theNewName, theNewBrand, theNewColor);

        private It should_have_the_new_brand = () =>
            sut.Brand.ShouldEqual(theNewBrand);

        private It should_have_the_new_color = () =>
            sut.Color.ShouldEqual(theNewColor);

        private It should_have_the_new_name = () =>
            sut.Name.ShouldEqual(theNewName);

        private It should_public_details_changed = () =>
            published<ArticleDetailsChanged>(p =>
                p.ArticleId == theArticleId.Id
                && p.Brand == theNewBrand
                && p.Color == theNewColor
                && Equals(p.Initiator, theManager.ToActor())
                && p.Name == theNewName
                && p.OwnerId == theOwner.OwnerId.Id);
    }

    [Subject(typeof (Article))]
    public class when_changing_description : article_concern
    {
        private static string theNewDescription;
        private Establish ctx = () => { theNewDescription = "The new description"; };

        private Because of = () =>
            sut.ChangeDescription(theManager, theNewDescription);

        private It should_have_new_description = () =>
            sut.Description.ShouldEqual(theNewDescription);

        private It should_publish_description_changed = () =>
            publisher.WasToldTo(x => x.Publish(
                Arg<DescriptionChanged>.Matches(p =>
                    p.ArticleId == theArticleId.Id
                    && p.Description == theNewDescription
                    && Equals(p.Initiator, theManager.ToActor())
                    && p.OwnerId == theOwner.OwnerId.Id)));
    }

    [Subject(typeof (Article))]
    public class when_changing_specification : article_concern
    {
        private static string theNewSpecification;
        private Establish ctx = () => theNewSpecification = "The new specification";

        private Because of = () => sut.ChangeSpecification(theManager, theNewSpecification);

        private It should_have_new_specification = () =>
            sut.Specification.ShouldEqual(theNewSpecification);

        private It should_publish_specification_changed = () =>
            publisher.WasToldTo(x => x.Publish(
                Arg<SpecificationChanged>.Matches(p =>
                    p.ArticleId == theArticleId.Id
                    && Equals(p.Initiator, theManager.ToActor())
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
            sut.ChangeGrossStock(theManager, theNewGrossStock);

        private It should_have_the_new_gross_stock = () =>
            sut.GrossStock.ShouldEqual(theNewGrossStock);

        private It should_publish_gross_stock_changed = () =>
            published<GrossStockChanged>(p =>
                p.ArticleId == theArticleId.Id
                && Equals(p.Initiator, theManager.ToActor())
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

        private Because of = () =>
            sut.ChangePrices(theManager, theNewPublicPrice, theNewMemberPrice);


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
                    p.ArticleId == theArticleId.Id
                    && Equals(p.Initiator, theManager.ToActor())
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
                    p.ArticleId == theArticleId.Id
                    && Equals(p.Initiator, theManager.ToActor())
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
                    p.ArticleId == theArticleId.Id
                    && Equals(p.Initiator, theManager.ToActor())
                    && p.MemberPrice == theNewMemberPrice
                    && p.PublicPrice == theNewPublicPrice)));
        }
    }

    [Subject(typeof (Article))]
    public class when_adding_an_image : article_concern
    {
        private Because of = () =>
            sut.AddImage(theManager, "fileName", "fileType", 123456);

        private It should_add_to_images_collection = () =>
            sut.Images.ShouldNotBeEmpty();

        private It should_publish_image_added = () =>
            published<ImageAdded>(p =>
                p.IsPreviewImage
                && p.ArticleId == theArticleId.Id
                && p.FileLength == 123456
                && p.FileName == "fileName"
                && p.FileType == "fileType"
                && Equals(p.Initiator, theManager.ToActor())
                && p.OwnerId == theOwner.OwnerId.Id);

        private It should_set_is_preview = () =>
            sut.Images.First().IsPreview.ShouldBeTrue();
    }

    [Subject(typeof (Article))]
    public class when_adding_a_second_image : article_concern
    {
        private Establish ctx = () => sut_setup.run(sut =>
            sut.AddImage(theManager, "first.jpg", "image/jpeg", 1234));

        private Because of = () =>
            sut.AddImage(theManager, "second.jpg", "image/jpeg", 2345);

        private It should_not_set_is_preview = () =>
            sut.Images.Single(p => p.FileName == "second.jpg").IsPreview.ShouldBeFalse();
    }

    [Subject(typeof (Article))]
    public class when_adding_a_second_image_with_same_name : article_concern
    {
        private static string theFileName = "file.jpg";

        private Establish ctx = () => sut_setup.run(sut =>
            sut.AddImage(theManager, theFileName, "image/jpeg", 1234));

        private Because of = () => spec.catch_exception(() =>
            sut.AddImage(theManager, theFileName, "image/jpeg", 1234));

        private It should_throw_exception_with_message = () =>
            spec.exception_thrown.Message.ShouldEqual("Image with file name file.jpg already exists.");

        private It should_throw_invalid_operation_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<InvalidOperationException>();
    }

    [Subject(typeof (Article))]
    public class when_removing_an_image : article_concern
    {
        private Establish ctx = () => sut_setup.run(x =>
        {
            x.AddImage(theManager, "first.jpg", "image/jpeg", 1234);
            x.AddImage(theManager, "second.jpg", "image/jpeg", 2345);
            x.AddImage(theManager, "third.jpg", "image/jpeg", 3456);
        });

        private Because of = () =>
            sut.RemoveImage(theManager, "first.jpg");

        private It should_publish_image_removed = () =>
            published<ImageRemoved>(p =>
                p.ArticleId == theArticleId.Id
                && p.FileName == "first.jpg"
                && Equals(p.Initiator, theManager.ToActor())
                && p.OwnerId == theOwner.OwnerId.Id);

        private It should_publish_preview_image_changed = () =>
            published<PreviewImageChanged>(p =>
                p.ArticleId == theArticleId.Id
                && p.FileLength == 2345
                && p.FileName == "second.jpg"
                && p.FileType == "image/jpeg"
                && Equals(p.Initiator, theManager.ToActor())
                && p.OwnerId == theOwner.OwnerId.Id);

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
            x.AddImage(theManager, "first.gif", "image/gif", 1234);
            x.AddImage(theManager, "second.jpg", "image/jpeg", 2345);
        });

        private Because of = () =>
            sut.SetPreviewImage(theManager, "second.jpg");

        private It should_publish_preview_image_changed = () =>
            published<PreviewImageChanged>(p =>
                p.ArticleId == theArticleId.Id
                && p.FileLength == 2345
                && p.FileName == "second.jpg"
                && p.FileType == "image/jpeg"
                && Equals(p.Initiator, theManager.ToActor())
                && p.OwnerId == theOwner.OwnerId.Id);

        private It should_remove_preview_flag = () =>
            sut.Images.Single(p => p.FileName == "first.gif").IsPreview.ShouldBeFalse();

        private It should_set_preview_flag = () =>
            sut.Images.Single(p => p.FileName == "second.jpg").IsPreview.ShouldBeTrue();
    }


    [Subject(typeof (Article))]
    public class when_tagging_an_untagged_product : article_concern
    {
        private Because of = () =>
            sut.Tag(theManager, "tag1");

        private It should_contain_one_tag = () =>
            sut.Tags.Count.ShouldEqual(1);

        private It should_contain_only_the_tag = () =>
            sut.Tags.ShouldContainOnly(new Tag("tag1"));

        private It should_publish_product_tagged = () =>
            published<ProductTagged>(p => p.TagName == "tag1");
    }

    [Subject(typeof (Article))]
    public class when_tagging_a_product_with_the_same_tag_twice : article_concern
    {
        private Establish ctx = () =>
            sut_setup.run(sut =>
                sut.Tag(theManager, "tag"));

        private Because of = () =>
            spec.catch_exception(() =>
                sut.Tag(theManager, "tag"));

        private It should_throw_invalid_operation_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<InvalidOperationException>();
    }

    [Subject(typeof (Article))]
    public class when_untagging_a_product_when_the_tag_is_not_present : article_concern
    {
        private Because of = () =>
            spec.catch_exception(() =>
                sut.Untag(theManager, "tag"));

        private It should_throw_invalid_operation_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<InvalidOperationException>();
    }

    [Subject(typeof (Article))]
    public class when_untagging_a_product : article_concern
    {
        private Establish ctx = () => sut_setup.run(sut =>
        {
            sut.Tag(theManager, "tag1");
            sut.Tag(theManager, "tag2");
        });

        private Because of = () =>
            sut.Untag(theManager, "tag1");

        private It should_remove_tag = () =>
            sut.Tags.ShouldNotContain(new Tag("tag1"));

        private It should_publish_product_untagged = () =>
            published<ProductUntagged>(p =>
                p.TagName == "tag1");
    }
}