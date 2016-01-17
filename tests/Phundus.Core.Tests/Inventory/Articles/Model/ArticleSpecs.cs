namespace Phundus.Tests.Inventory.Articles.Model
{
    using System.Linq;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Owners;
    using Rhino.Mocks;

    public class article_concern : aggregate_concern_new<Article>
    {
        protected static inventory_factory make;
        protected static Owner theOwner;
        protected static StoreId theStoreId;

        private Establish ctx = () =>
        {
            make = new inventory_factory(fake);

            theOwner = make.Owner();
            theStoreId = new StoreId();
            sut_factory.create_using(() => new Article(theOwner, theStoreId, "Name", 1));
        };
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