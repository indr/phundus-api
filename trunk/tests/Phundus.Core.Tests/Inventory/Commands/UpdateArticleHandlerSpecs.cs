namespace Phundus.Tests.Inventory.Articles.Commands
{
    using System;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Commands;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Authorization;
    using Phundus.Inventory.Model;

    [Subject(typeof (UpdateArticleHandler))]
    public class when_update_article_command_is_handled :
        article_command_handler_concern<UpdateArticle, UpdateArticleHandler>
    {
        private static Article theArticle;
        private static Owner theOwner;
        private static string theName = "The name";
        private static string theBrand = "The brand";
        private static string theColor = "The color";
        private static int theGrossStock = 123;

        private Establish ctx = () =>
        {
            theArticle = make.Article();
            theOwner = theArticle.Owner;
            articleRepository.WhenToldTo(x => x.GetById(theArticle.Id)).Return(theArticle);

            command = new UpdateArticle(theInitiatorId, theArticle.ArticleShortId.Id, theName, theBrand, theColor, theGrossStock);
        };

        private It should_enforce_initiator_to_manage_articles = () =>
            EnforcedInitiatorTo<ManageArticlesAccessObject>(p => Equals(p.OwnerId, theOwner.OwnerId));

        private It should_tell_article_to_change_details = () =>
            theArticle.WasToldTo(x => x.ChangeDetails(theInitiator, theName, theBrand, theColor));

        private It should_tell_article_to_change_gross_stock = () =>
            theArticle.WasToldTo(x => x.ChangeGrossStock(theInitiator, theGrossStock));
    }
}