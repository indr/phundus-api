namespace Phundus.Tests.Inventory.Articles.Commands
{
    using System;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Commands;
    using Phundus.Inventory.Articles.Model;

    [Subject(typeof (UpdateArticleHandler))]
    public class when_update_article_command_is_handled :
        article_command_handler_concern<UpdateArticle, UpdateArticleHandler>
    {
        private const int articleId = 1;
        private static Guid ownerId;
        private static Owner owner;
        private static StoreId storeId;
        private static UserId initiatorId;

        private static Article article;

        public Establish c = () =>
        {
            initiatorId = new UserId();
            ownerId = Guid.NewGuid();
            owner = new Owner(new OwnerId(ownerId), "Owner", OwnerType.Organization);
            article = make.Article(owner);
            articleRepository.WhenToldTo(x => x.GetById(articleId)).Return(article);

            command = new UpdateArticle();
            command.InitiatorId = initiatorId;
            command.ArticleId = articleId;
        };

        private It should_ask_for_chief_privileges = () =>
            memberInRole.WasToldTo(x => x.ActiveManager(ownerId, initiatorId));

        private It should_publish_article_updated = () =>
            Published<ArticleUpdated>(p => p != null);
    }
}