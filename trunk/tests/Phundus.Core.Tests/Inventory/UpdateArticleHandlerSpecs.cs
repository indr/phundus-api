namespace Phundus.Core.Tests.Inventory
{
    using Core.Inventory.Commands;
    using Core.Inventory.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    public class when_update_article_is_handled : handler_concern<UpdateArticle, UpdateArticleHandler>
    {
        private static Article article = new Article(organizationId, "Name");

        private const int articleId = 1;
        private const int organizationId = 2;
        private const int initiatorId = 3;

        public Establish c = () =>
        {
            repository.WhenToldTo(x => x.ById(articleId)).Return(article);

            command = new UpdateArticle();
            command.InitiatorId = initiatorId;
            command.ArticleId = articleId;
        };

        private It should_ask_for_chief_privilegs = () => memberInRole.WasToldTo(x => x.ActiveChief(organizationId, initiatorId));

        private It should_publich_article_updated =
            () => publisher.WasToldTo(x => x.Publish(Arg<ArticleUpdated>.Is.NotNull));
    }
}