namespace Phundus.Core.Tests.Inventory
{
    using System;
    using System.Linq;
    using Core.Inventory.Articles.Commands;
    using Core.Inventory.Articles.Model;
    using Core.Inventory.Owners;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof(AddImageHandler))]
    public class when_add_image_is_handled : article_handler_concern<AddImage, AddImageHandler>
    {
        private static Guid ownerId;
        private static Owner owner;
        private const int initiatorId = 2;
        private const int articleId = 3;
        private const string imageFileName = "Image.jpg";
        
        private static Article article;
        
        private Establish c = () =>
        {
            ownerId = Guid.NewGuid();
            owner = new Owner(new OwnerId(ownerId), "Owner");
            article = new Article(1, owner, "Name");
            repository.setup(x => x.GetById(articleId)).Return(article);

            command = new AddImage
            {
                ArticleId = articleId,
                InitiatorId = initiatorId,
                FileName = imageFileName
            };
        };

        private It should_add_image = () => article.Images.FirstOrDefault(p => p.FileName == imageFileName).ShouldNotBeNull();

        private It should_ask_for_chief_privilegs =
            () => memberInRole.WasToldTo(x => x.ActiveChief(ownerId, initiatorId));

        private It should_publish_image_added = () => publisher.WasToldTo(x => x.Publish(Arg<ImageAdded>.Is.NotNull));

        private It should_set_image_id = () => command.ImageId.HasValue.ShouldBeTrue();
    }
}