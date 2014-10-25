namespace Phundus.Core.Inventory.Articles.Commands
{
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
            var article = ArticleRepository.GetById(command.ArticleId);

            MemberInRole.ActiveChief(article.OrganizationId, command.InitiatorId);

            var image = article.AddImage(command.FileName, command.Type, command.Length);
            command.ImageId = image.Id;

            EventPublisher.Publish(new ImageAdded());
        }
    }
}