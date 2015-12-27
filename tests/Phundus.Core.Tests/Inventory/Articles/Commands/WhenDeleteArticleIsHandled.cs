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

    [Subject(typeof (DeleteArticleHandler))]
    public class when_delete_article_is_handled : article_handler_concern<DeleteArticle, DeleteArticleHandler>
    {
        private static Guid ownerId;
        private static Owner owner;
        private const int initiatorId = 2;
        private const int articleId = 3;

        private static Article article;

        private Establish c = () =>
        {
            ownerId = Guid.NewGuid();
            owner = new Owner(new OwnerId(ownerId), "Owner");
            article = new Article(owner, "Name");
            repository.setup(x => x.GetById(articleId)).Return(article);

            command = new DeleteArticle
            {
                ArticleId = articleId,
                InitiatorId = initiatorId
            };
        };

        private It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(ownerId, initiatorId));

        private It should_ask_for_removal = () => repository.WasToldTo(x => x.Remove(article));

        private It should_publish_article_deleted =
            () => publisher.WasToldTo(x => x.Publish(Arg<ArticleDeleted>.Is.NotNull));
    }
}