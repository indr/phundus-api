namespace Phundus.Core.Tests.Inventory
{
    using System;
    using Core.Inventory.Articles.Commands;
    using Core.Inventory.Articles.Model;
    using Core.Inventory.Owners;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof(RemoveImageHandler))]
    public class when_remove_image_is_handled : article_handler_concern<RemoveImage, RemoveImageHandler>
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
            article.AddImage(imageFileName, "image/jpeg", 1024);
            repository.setup(x => x.GetById(articleId)).Return(article);

            command = new RemoveImage
            {
                ArticleId = articleId,
                ImageFileName = imageFileName,
                InitiatorId = initiatorId
            };
        };

        private It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(ownerId, initiatorId));

        private It should_remove_image = () => article.Images.ShouldBeEmpty();

        private It should_publish_image_removed =
            () => publisher.WasToldTo(x => x.Publish(Arg<ImageRemoved>.Is.NotNull));
    }
}