namespace Phundus.Tests.Inventory.Application
{
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Model;

    [Subject(typeof (UpdateArticleHandler))]
    public class when_update_article_command_is_handled :
        article_command_handler_concern<UpdateArticle, UpdateArticleHandler>
    {
        private static Article theArticle;        
        private static string theName = "The name";
        private static string theBrand = "The brand";
        private static string theColor = "The color";
        private static int theGrossStock = 123;

        private Establish ctx = () =>
        {
            theArticle = make.Article(theOwner);            
            articleRepository.WhenToldTo(x => x.GetById(theArticle.ArticleId)).Return(theArticle);

            command = new UpdateArticle(theInitiatorId, theArticle.ArticleId, theName, theBrand, theColor, theGrossStock);
        };

        private It should_tell_article_to_change_details = () =>
            theArticle.WasToldTo(x => x.ChangeDetails(theManager, theName, theBrand, theColor));

        private It should_tell_article_to_change_gross_stock = () =>
            theArticle.WasToldTo(x => x.ChangeGrossStock(theManager, theGrossStock));
    }
}