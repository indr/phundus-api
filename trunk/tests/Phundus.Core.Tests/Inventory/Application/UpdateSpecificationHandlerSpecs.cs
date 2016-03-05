namespace Phundus.Tests.Inventory.Application
{
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Authorization;
    using Phundus.Inventory.Model;
    using Rhino.Mocks;

    [Subject(typeof (UpdateSpecificationHandler))]
    public class when_handling_update_specification :
        article_command_handler_concern<UpdateSpecification, UpdateSpecificationHandler>
    {
        private static Article theArticle;
        private static string theSpecification = "The description";
        private static Owner theOwner;

        private Establish ctx = () =>
        {
            theArticle = make.Article();
            theOwner = theArticle.Owner;
            articleRepository.WhenToldTo(x => x.GetById(theArticle.ArticleId)).Return(theArticle);

            command = new UpdateSpecification(theInitiatorId, theArticle.ArticleId, theSpecification);
        };

        private It should_authorize_initiator_to_manage_articles = () =>
            authorize.WasToldTo(x =>
                x.Enforce(Arg<InitiatorId>.Is.Equal(theInitiatorId),
                    Arg<ManageArticlesAccessObject>.Matches(p => Equals(p.OwnerId, theOwner.OwnerId))));

        private It should_call_change_description = () =>
            theArticle.WasToldTo(x => x.ChangeSpecification(theInitiator, theSpecification));
    }
}