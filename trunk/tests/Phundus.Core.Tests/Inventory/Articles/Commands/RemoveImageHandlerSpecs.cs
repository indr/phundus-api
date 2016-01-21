namespace Phundus.Tests.Inventory.Articles.Commands
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Commands;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Owners;
    using Rhino.Mocks;
    using Owner = Phundus.Inventory.Owners.Owner;

    [Subject(typeof (RemoveImageHandler))]
    public class when_remove_image_is_handled : article_command_handler_concern<RemoveImage, RemoveImageHandler>
    {
        private static Owner theOwner;
        private static Article theArticle;
        private static string theFileName = "file.jpg";

        private Establish ctx = () =>
        {
            theOwner = make.Owner();
            theArticle = make.Article(theOwner);
            articleRepository.setup(x => x.GetById(theArticle.Id)).Return(theArticle);

            command = new RemoveImage(theInitiatorId, new ArticleId(theArticle.Id), theFileName);
        };

        private It should_ask_for_chief_privileges = () =>
            memberInRole.WasToldTo(x => x.ActiveManager(theOwner.OwnerId.Id, theInitiatorId));

        private It should_tell_article_to_remove_image = () =>
            theArticle.WasToldTo(x => x.RemoveImage(theFileName));
    }
}