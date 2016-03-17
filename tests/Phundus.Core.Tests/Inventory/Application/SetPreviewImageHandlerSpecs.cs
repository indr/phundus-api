namespace Phundus.Tests.Inventory.Application
{
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Articles.Model;

    [Subject(typeof (SetPreviewImageHandler))]
    public class when_handling_set_preview_image_command :
        article_command_handler_concern<SetPreviewImage, SetPreviewImageHandler>
    {
        private static Article theArticle;
        private static string theFileName = "theFileName.jpg";

        private Establish ctx = () =>
        {
            theArticle = make.Article(theOwner);
            articleRepository.setup(x => x.GetById(theArticle.ArticleId)).Return(theArticle);

            command = new SetPreviewImage(theInitiatorId, theArticle.ArticleId, theFileName);
        };

        private It should_call_set_preview_image = () =>
            theArticle.received(x => x.SetPreviewImage(theManager, theFileName));
    }
}