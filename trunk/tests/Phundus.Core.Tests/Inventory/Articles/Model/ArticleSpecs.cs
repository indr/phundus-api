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

        private It should_add_an_image = () =>
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
}