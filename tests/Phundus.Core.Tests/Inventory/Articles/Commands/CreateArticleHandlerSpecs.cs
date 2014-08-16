namespace Phundus.Core.Tests.Inventory
{
    using Core.Inventory.Articles.Commands;
    using Core.Inventory.Articles.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (CreateArticleHandler))]
    public class when_create_article_is_handled : article_handler_concern<CreateArticle, CreateArticleHandler>
    {
        public Establish c = () =>
        {
            repository.setup(x => x.Add(Arg<Article>.Is.Anything)).Return(1);

            command = new CreateArticle();
            command.InitiatorId = 2;
            command.OrganizationId = 1;
        };

        public It should_add_to_repository = () => repository.WasToldTo(x => x.Add(Arg<Article>.Is.NotNull));

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(1, 2));

        public It should_publish_article_created =
            () => publisher.WasToldTo(x => x.Publish(Arg<ArticleCreated>.Is.NotNull));

        public It should_set_article_id = () => command.ArticleId.ShouldNotBeNull();
    }
}