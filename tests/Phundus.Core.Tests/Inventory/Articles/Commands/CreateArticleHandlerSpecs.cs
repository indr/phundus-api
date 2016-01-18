namespace Phundus.Tests.Inventory.Articles.Commands
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

    [Subject(typeof (CreateArticleHandler))]
    public class when_create_article_is_handled : article_handler_concern<CreateArticle, CreateArticleHandler>
    {
        private static UserId initiatorId;
        private static OwnerId ownerId;

        public Establish c = () =>
        {
            initiatorId = new UserId();
            ownerId = new OwnerId(Guid.NewGuid());
            ownerService.setup(x => x.GetById(ownerId)).Return(new Owner(ownerId, "Owner"));
            articleRepository.setup(x => x.Add(Arg<Article>.Is.Anything)).Return(1);

            command = new CreateArticle(initiatorId, ownerId, new StoreId(), "Name", 0);
        };

        public It should_add_to_repository = () => articleRepository.WasToldTo(x => x.Add(Arg<Article>.Is.NotNull));

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(ownerId, initiatorId));

        public It should_publish_article_created =
            () => publisher.WasToldTo(x => x.Publish(Arg<ArticleCreated>.Is.NotNull));

        public It should_set_article_id = () => command.ResultingArticleId.ShouldNotBeNull();
    }
}