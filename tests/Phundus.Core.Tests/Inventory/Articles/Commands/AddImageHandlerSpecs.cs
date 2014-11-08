namespace Phundus.Core.Tests.Inventory
{
    using System.Linq;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Catalog;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof(AddImageHandler))]
    public class when_add_image_is_handled : article_handler_concern<AddImage, AddImageHandler>
    {
        private const int organizationId = 1;
        private const int initiatorId = 2;
        private const int articleId = 3;
        private const string imageFileName = "Image.jpg";
        
        private static readonly Article article = new Article(organizationId, "Name");
        
        private Establish c = () =>
        {
            repository.setup(x => x.GetById(articleId)).Return(article);

            command = new AddImage
            {
                ArticleId = articleId,
                InitiatorId = initiatorId,
                FileName = imageFileName
            };
        };

        private It should_add_image = () => article.Images.FirstOrDefault(p => p.FileName == imageFileName).ShouldNotBeNull();

        private It should_ask_for_chief_privilegs =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organizationId, initiatorId));

        private It should_publish_image_added = () => publisher.WasToldTo(x => x.Publish(Arg<ImageAdded>.Is.NotNull));

        private It should_set_image_id = () => command.ImageId.HasValue.ShouldBeTrue();
    }
}