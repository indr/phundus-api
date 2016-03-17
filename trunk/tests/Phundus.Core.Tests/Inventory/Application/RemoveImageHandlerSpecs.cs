namespace Phundus.Tests.Inventory.Application
{
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Articles.Model;    

    [Subject(typeof (RemoveImageHandler))]
    public class when_remove_image_is_handled : article_command_handler_concern<RemoveImage, RemoveImageHandler>
    {        
        private static Article theArticle;
        private static string theFileName = "file.jpg";

        private Establish ctx = () =>
        {            
            theArticle = make.Article(theOwner);
            articleRepository.setup(x => x.GetById(theArticle.ArticleId)).Return(theArticle);

            command = new RemoveImage(theInitiatorId, theArticle.ArticleId, theFileName);
        };

        private It should_tell_article_to_remove_image = () =>
            theArticle.received(x => x.RemoveImage(theManager, theFileName));
    }
}