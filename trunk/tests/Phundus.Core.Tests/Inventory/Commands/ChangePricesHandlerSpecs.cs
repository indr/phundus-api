namespace Phundus.Tests.Inventory.Commands
{
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Commands;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Authorization;
    using Phundus.Inventory.Model;
    using Rhino.Mocks;

    [Subject(typeof(ChangePrices))]
    public class when_handling_change_prices : article_command_handler_concern<ChangePrices, ChangePricesHandler>
    {
        private static Article theArticle;
        private static decimal thePublicPrice = 1.11m;
        private static decimal? theMemberPrice = 2.22m;
        private static Owner theOwner;

        private Establish ctx = () =>
        {
            theOwner = make.Owner();
            theArticle = make.Article(theOwner);
            articleRepository.WhenToldTo(x => x.GetById(theArticle.Id)).Return(theArticle);
            command = new ChangePrices(theInitiatorId, theArticle.Id, thePublicPrice, theMemberPrice);
        };

        private It should_authorize_initiator_to_manage_articles = () =>
            authorize.WasToldTo(x =>
                x.Enforce(Arg<InitiatorId>.Is.Equal(theInitiatorId),
                    Arg<ManageArticlesAccessObject>.Matches(p => Equals(p.OwnerId, theOwner.OwnerId))));

        private It should_call_change_prices = () =>
            theArticle.WasToldTo(x => x.ChangePrices(theInitiator, thePublicPrice, theMemberPrice));
    }
}