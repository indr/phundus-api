namespace Phundus.Tests.Inventory.Application
{
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Authorization;
    using Phundus.Inventory.Model;

    [Subject(typeof (DeleteArticleHandler))]
    public class when_delete_article_command_is_handled :
        article_command_handler_concern<DeleteArticle, DeleteArticleHandler>
    {
        private static Owner theOwner;
        private static Article theArticle;

        private Establish c = () =>
        {
            theArticle = make.Article();
            theOwner = theArticle.Owner;
            articleRepository.setup(x => x.GetById(theArticle.ArticleShortId)).Return(theArticle);

            command = new DeleteArticle(theInitiatorId, theArticle.Id);
        };

        private It should_enforce_initiator_to_manage_articles = () =>
            enforceInitiatorTo<ManageArticlesAccessObject>(p => Equals(p.OwnerId, theOwner.OwnerId));

        private It should_publish_article_deleted = () =>
            published<ArticleDeleted>(p =>
                p.ArticleId == theArticle.ArticleId.Id
                && Equals(p.Initiator, theInitiator)
                && p.OwnerId == theOwner.OwnerId.Id);

        private It should_tell_repository_to_remove = () => articleRepository.WasToldTo(x => x.Remove(theArticle));
    }
}