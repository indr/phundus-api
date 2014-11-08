namespace Phundus.Core.Inventory.Application.Commands
{
    using Cqrs;
    using Ddd;
    using Domain.Model.Catalog;
    using IdentityAndAccess.Queries;

    public class RemoveImage
    {
        public int ArticleId { get; set; }
        public string ImageFileName { get; set; }
        public int InitiatorId { get; set; }        
    }

    public class RemoveImageHandler : IHandleCommand<RemoveImage>
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(RemoveImage command)
        {
            var article = ArticleRepository.GetById(command.ArticleId);

            MemberInRole.ActiveChief(article.OrganizationId, command.InitiatorId);

            article.RemoveImage(command.ImageFileName);

            EventPublisher.Publish(new ImageRemoved());
        }
    }
}