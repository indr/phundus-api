namespace Phundus.Core.Tests.Inventory
{
    using System;
    using Core.Inventory.Articles.Commands;
    using Core.Inventory.Articles.Model;
    using Core.Inventory.Owners;
    using Core.Inventory.Stores.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof(UpdateArticleHandler))]
    public class when_update_article_is_handled : article_handler_concern<UpdateArticle, UpdateArticleHandler>
    {
        private static Guid ownerId;
        private static Owner owner;
        private static StoreId storeId;
        private const int articleId = 1;
        private const int initiatorId = 3;

        private static Article article;

        public Establish c = () =>
        {
            ownerId = Guid.NewGuid();
            storeId = new StoreId();
            owner = new Owner(new OwnerId(ownerId), "Owner");
            article = new Article(owner, storeId, "Name");
            repository.WhenToldTo(x => x.GetById(articleId)).Return(article);

            command = new UpdateArticle();
            command.InitiatorId = initiatorId;
            command.ArticleId = articleId;
        };

        private It should_ask_for_chief_privileges = () => memberInRole.WasToldTo(x => x.ActiveChief(ownerId, initiatorId));

        private It should_publich_article_updated =
            () => publisher.WasToldTo(x => x.Publish(Arg<ArticleUpdated>.Is.NotNull));
    }
}