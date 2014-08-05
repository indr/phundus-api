namespace Phundus.Core.Inventory.Commands
{
    using System.Linq;
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using Model;
    using Repositories;

    public class AddImage
    {
        public int ArticleId { get; set; }
        public int? ImageId { get; set; }
        public int InitiatorId { get; set; }

        public long Length { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
    }

    public class AddImageHandler : IHandleCommand<AddImage>
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(AddImage command)
        {
            var article = ArticleRepository.ById(command.ArticleId);
            if (article == null)
                throw new ArticleNotFoundException();

            MemberInRole.ActiveChief(article.OrganizationId, command.InitiatorId);

            var image = article.AddImage(command.FileName, command.Type, command.Length);
            command.ImageId = image.Id;

            EventPublisher.Publish(new ImageAdded());
        }
    }
}