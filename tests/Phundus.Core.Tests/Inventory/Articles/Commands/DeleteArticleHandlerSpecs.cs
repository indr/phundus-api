namespace Phundus.Tests.Inventory.Articles.Commands
{
    using System;
    using System.Linq.Expressions;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Commands;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Authorize;
    using Rhino.Mocks;

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
            articleRepository.setup(x => x.GetById(theArticle.Id)).Return(theArticle);

            command = new DeleteArticle(theInitiatorId, theArticle.Id);
        };

        private It should_authorize_initiator_to_manage_articles = () =>
            authorize.WasToldTo(x =>
                x.Enforce(Arg<InitiatorId>.Is.Equal(theInitiatorId),
                    Arg<ManageArticlesAccessObject>.Matches(p => Equals(p.OwnerId, theOwner.OwnerId))));


        private It should_publish_article_deleted = () =>
            Published<ArticleDeleted>(p => p.ArticleGuid == theArticle.ArticleGuid.Id
                           && Equals(p.Initiator, theInitiator)
                           && p.OwnerId == theOwner.OwnerId.Id);

        private It should_tell_repository_to_remove = () => articleRepository.WasToldTo(x => x.Remove(theArticle));
    }
}