namespace Phundus.Tests.Inventory.Application
{
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Model;

    [Subject(typeof (UpdateDescriptionHandler))]
    public class when_handling_change_description :
        article_command_handler_concern<UpdateDescription, UpdateDescriptionHandler>
    {
        private static Article theArticle;
        private static string theDescription = "The description";        

        private Establish ctx = () =>
        {
            theArticle = make.Article(theOwner);
            articleRepository.setup(x => x.GetById(theArticle.ArticleId)).Return(theArticle);

            command = new UpdateDescription(theInitiatorId, theArticle.ArticleId, theDescription);
        };

        private It should_call_change_description = () =>
            theArticle.received(x => x.ChangeDescription(theManager, theDescription));
    }
}