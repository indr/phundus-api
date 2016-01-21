namespace Phundus.Tests.Inventory.Articles.Commands
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Commands;
    using Phundus.Inventory.Articles.Model;
    using Rhino.Mocks;

    [Subject(typeof (DeleteArticleHandler))]
    public class when_delete_article_command_is_handled : article_command_handler_concern<DeleteArticle, DeleteArticleHandler>
    {
        private static UserId initiatorId;
        private const int articleId = 3;
        private static Guid ownerId;
        private static Owner owner;

        private static Article article;

        private Establish c = () =>
        {
            initiatorId = new UserId();
            ownerId = Guid.NewGuid();
            owner = new Owner(new OwnerId(ownerId), "Owner", OwnerType.Organization);
            article = new Article(owner, new StoreId(), "Name", 0);
            articleRepository.setup(x => x.GetById(articleId)).Return(article);

            command = new DeleteArticle
            {
                ArticleId = articleId,
                InitiatorId = initiatorId
            };
        };

        private It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveManager(ownerId, initiatorId));

        private It should_ask_for_removal = () => articleRepository.WasToldTo(x => x.Remove(article));

        private It should_publish_article_deleted =
            () => publisher.WasToldTo(x => x.Publish(Arg<ArticleDeleted>.Is.NotNull));
    }
}