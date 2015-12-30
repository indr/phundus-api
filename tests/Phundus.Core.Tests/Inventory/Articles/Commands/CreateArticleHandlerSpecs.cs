namespace Phundus.Core.Tests.Inventory
{
    using System;
    using Common.Domain.Model;
    using Core.Inventory.Articles.Commands;
    using Core.Inventory.Articles.Model;
    using Core.Inventory.Owners;
    using Core.Inventory.Stores.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (CreateArticleHandler))]
    public class when_create_article_is_handled : article_handler_concern<CreateArticle, CreateArticleHandler>
    {
        private static Guid organizationId;

        public Establish c = () =>
        {
            organizationId = Guid.NewGuid();
            ownerService.setup(x => x.GetById(organizationId)).Return(new Owner(new OwnerId(organizationId), "Owner"));
            repository.setup(x => x.Add(Arg<Article>.Is.Anything)).Return(1);

            command = new CreateArticle(new UserId(2), new OwnerId(organizationId), new StoreId(), "Name");
        };

        public It should_add_to_repository = () => repository.WasToldTo(x => x.Add(Arg<Article>.Is.NotNull));

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organizationId, 2));

        public It should_publish_article_created =
            () => publisher.WasToldTo(x => x.Publish(Arg<ArticleCreated>.Is.NotNull));

        public It should_set_article_id = () => command.ResultingArticleId.ShouldNotBeNull();
    }
}