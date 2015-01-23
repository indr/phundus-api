namespace Phundus.Core.Inventory.Application.Commands
{
    using Common.Cqrs;
    using Cqrs;
    using Ddd;
    using Domain.Model.Catalog;
    using IdentityAndAccess.Queries;

    public class AddImage : ICommand
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