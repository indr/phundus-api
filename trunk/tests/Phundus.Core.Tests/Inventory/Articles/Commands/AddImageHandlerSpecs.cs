﻿namespace Phundus.Tests.Inventory.Articles.Commands
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Commands;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Owners;
    using Rhino.Mocks;

    [Subject(typeof (AddImageHandler))]
    public class when_add_image_is_handled : article_handler_concern<AddImage, AddImageHandler>
    {
        private static Owner theOwner;
        private static Article theArticle;
        private static int theImageId = 123;

        private Establish ctx = () =>
        {
            theOwner = make.Owner();
            var theImage = fake.an<Image>();
            theImage.WhenToldTo(x => x.Id).Return(theImageId);
            theArticle = make.Article(theOwner);
            theArticle.WhenToldTo(
                x => x.AddImage(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<long>.Is.Anything))
                .Return(theImage);
            articleRepository.setup(x => x.GetById(theArticle.Id)).Return(theArticle);


            command = new AddImage(theInitiatorId, new ArticleId(theArticle.Id), "file.jpg", "image/jpeg", 12345);
        };

        private It should_ask_for_chief_privilegs = () =>
            memberInRole.WasToldTo(x => x.ActiveChief(theOwner.OwnerId.Id, theInitiatorId));

        private It should_set_image_id = () =>
            command.ResultingImageId.ShouldEqual(theImageId);

        private It tell_article_to_add_image = () =>
            theArticle.WasToldTo(x => x.AddImage("file.jpg", "image/jpeg", 12345));
    }
}