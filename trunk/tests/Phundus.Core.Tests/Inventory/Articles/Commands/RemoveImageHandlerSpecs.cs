﻿namespace Phundus.Tests.Inventory.Articles.Commands
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Commands;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Owners;
    using Rhino.Mocks;

    [Subject(typeof (RemoveImageHandler))]
    public class when_remove_image_is_handled : article_handler_concern<RemoveImage, RemoveImageHandler>
    {
        private static UserGuid initiatorId;
        private const int articleId = 3;
        private const string imageFileName = "Image.jpg";
        private static Guid ownerId;
        private static Owner owner;

        private static Article article;

        private Establish c = () =>
        {
            initiatorId = new UserGuid();
            ownerId = Guid.NewGuid();
            owner = new Owner(new OwnerId(ownerId), "Owner");
            article = new Article(owner, new StoreId(), "Name", 0);
            article.AddImage(imageFileName, "image/jpeg", 1024);
            articleRepository.setup(x => x.GetById(articleId)).Return(article);

            command = new RemoveImage(theInitiatorId, new ArticleId(articleId), imageFileName);
        };

        private It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(ownerId, initiatorId));

        private It should_publish_image_removed =
            () => publisher.WasToldTo(x => x.Publish(Arg<ImageRemoved>.Is.NotNull));

        private It should_remove_image = () => article.Images.ShouldBeEmpty();
    }
}