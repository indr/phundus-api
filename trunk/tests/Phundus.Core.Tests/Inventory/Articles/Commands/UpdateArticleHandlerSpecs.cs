namespace Phundus.Tests.Inventory.Articles.Commands
{
    using System;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Commands;
    using Phundus.Inventory.Articles.Model;    
    using Rhino.Mocks;

    [Subject(typeof(UpdateArticleHandler))]
    public class when_update_article_command_is_handled : article_command_handler_concern<UpdateArticle, UpdateArticleHandler>
    {
        private static Guid ownerId;
        private static Owner owner;
        private static StoreId storeId;
        private const int articleId = 1;
        private static UserId initiatorId;

        private static Article article;

        public Establish c = () =>
        {
            initiatorId = new UserId();
            ownerId = Guid.NewGuid();
            storeId = new StoreId();
            owner = new Owner(new OwnerId(ownerId), "Owner", OwnerType.Organization);
            article = new Article(owner, storeId, "Name", 0);
            articleRepository.WhenToldTo(x => x.GetById(articleId)).Return(article);

            command = new UpdateArticle();
            command.InitiatorId = initiatorId;
            command.ArticleId = articleId;
        };

        private It should_ask_for_chief_privileges = () => memberInRole.WasToldTo(x => x.ActiveManager(ownerId, initiatorId));

        private It should_publish_article_updated =
            () => publisher.WasToldTo(x => x.Publish(Arg<ArticleUpdated>.Is.NotNull));
    }
}