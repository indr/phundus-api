namespace Phundus.Core.Inventory.Commands
{
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using Model;
    using Repositories;

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
            var article = ArticleRepository.ById(command.ArticleId);
            if (article == null)
                throw new ArticleNotFoundException();

            MemberInRole.ActiveChief(article.OrganizationId, command.InitiatorId);

            article.RemoveImage(command.ImageFileName);

            EventPublisher.Publish(new ImageRemoved());
        }
    }
}