namespace Phundus.Tests.Inventory.Application
{
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Articles.Model;

    [Subject(typeof (DeleteArticleHandler))]
    public class when_delete_article_command_is_handled :
        article_command_handler_concern<DeleteArticle, DeleteArticleHandler>
    {
        private static Article theArticle;

        private Establish c = () =>
        {
            theArticle = make.Article(theOwner);
            articleRepository.setup(x => x.GetById(theArticle.ArticleId)).Return(theArticle);

            command = new DeleteArticle(theInitiatorId, theArticle.ArticleId);
        };

        private It should_publish_article_deleted = () =>
            published<ArticleDeleted>(p =>
                p.ArticleId == theArticle.ArticleId.Id
                && Equals(p.Initiator, theManager)
                && p.OwnerId == theOwner.OwnerId.Id);

        private It should_tell_repository_to_remove = () =>
            articleRepository.received(x => x.Remove(theArticle));
    }
}