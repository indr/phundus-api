namespace Phundus.Tests.Inventory.Application
{
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Model;

    [Subject(typeof (ChangePrices))]
    public class when_handling_change_prices : article_command_handler_concern<ChangePrices, ChangePricesHandler>
    {
        private static Article theArticle;
        private static decimal thePublicPrice = 1.11m;
        private static decimal? theMemberPrice = 2.22m;        

        private Establish ctx = () =>
        {            
            theArticle = make.Article(theOwner);
            articleRepository.setup(x => x.GetById(theArticle.ArticleId)).Return(theArticle);
            command = new ChangePrices(theInitiatorId, theArticle.ArticleId, thePublicPrice, theMemberPrice);
        };

        private It should_call_change_prices = () =>
            theArticle.received(x => x.ChangePrices(theManager, thePublicPrice, theMemberPrice));
    }
}