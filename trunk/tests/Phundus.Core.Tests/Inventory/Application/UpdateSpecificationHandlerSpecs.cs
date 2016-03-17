namespace Phundus.Tests.Inventory.Application
{
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Model;

    [Subject(typeof (UpdateSpecificationHandler))]
    public class when_handling_update_specification :
        article_command_handler_concern<UpdateSpecification, UpdateSpecificationHandler>
    {
        private static Article theArticle;
        private static string theSpecification = "The description";        

        private Establish ctx = () =>
        {
            theArticle = make.Article(theOwner);            
            articleRepository.setup(x => x.GetById(theArticle.ArticleId)).Return(theArticle);

            command = new UpdateSpecification(theInitiatorId, theArticle.ArticleId, theSpecification);
        };

        private It should_call_change_description = () =>
            theArticle.received(x => x.ChangeSpecification(theManager, theSpecification));
    }
}