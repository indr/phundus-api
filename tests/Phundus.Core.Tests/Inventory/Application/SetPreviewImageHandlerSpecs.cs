namespace Phundus.Tests.Inventory.Application
{
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Authorization;

    [Subject(typeof (SetPreviewImageHandler))]
    public class when_handling_set_preview_image_command :
        article_command_handler_concern<SetPreviewImage, SetPreviewImageHandler>
    {
        private static Article theArticle;
        private static string theFileName = "theFileName.jpg";

        private Establish ctx = () =>
        {
            theArticle = make.Article();
            articleRepository.WhenToldTo(x => x.GetById(theArticle.ArticleShortId)).Return(theArticle);

            command = new SetPreviewImage(theInitiatorId, theArticle.ArticleShortId, theFileName);
        };

        private It should_enforce_initiator_to_manage_articles = () =>
            enforceInitiatorTo<ManageArticlesAccessObject>(p => Equals(p.OwnerId, theArticle.Owner.OwnerId));

        private It should_call_set_preview_image = () =>
            theArticle.WasToldTo(x => x.SetPreviewImage(theInitiator, theFileName));
    }
}