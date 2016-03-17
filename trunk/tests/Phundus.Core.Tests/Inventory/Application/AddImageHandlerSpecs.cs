namespace Phundus.Tests.Inventory.Application
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Model.Articles;
    using Rhino.Mocks;

    [Subject(typeof (AddImageHandler))]
    public class when_add_image_is_handled : article_command_handler_concern<AddImage, AddImageHandler>
    {        
        private static Article theArticle;
        private static int theImageId = 123;

        private Establish ctx = () =>
        {            
            var theImage = fake.an<Image>();
            theImage.setup(x => x.Id).Return(theImageId);
            theArticle = make.Article(theOwner);
            theArticle.setup(x =>
                x.AddImage(Arg<Manager>.Is.Anything, Arg<string>.Is.Anything, Arg<string>.Is.Anything,
                    Arg<long>.Is.Anything))
                .Return(theImage);
            articleRepository.setup(x => x.GetById(theArticle.ArticleId)).Return(theArticle);

            command = new AddImage(theInitiatorId, theArticle.ArticleId, "file.jpg", "image/jpeg", 12345);
        };

        private It tell_article_to_add_image = () =>
            theArticle.received(x => x.AddImage(theManager, "file.jpg", "image/jpeg", 12345));
    }
}