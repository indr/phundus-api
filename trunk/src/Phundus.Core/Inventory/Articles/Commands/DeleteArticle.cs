namespace Phundus.Inventory.Articles.Commands
{
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using IdentityAccess.Queries;
    using Model;
    using Repositories;

    public class DeleteArticle
    {
        public int ArticleId { get; set; }
        public UserId InitiatorId { get; set; }
    }

    public class DeleteArticleHandler : IHandleCommand<DeleteArticle>
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(DeleteArticle command)
        {
            var article = ArticleRepository.GetById(command.ArticleId);

            MemberInRole.ActiveManager(article.Owner.OwnerId.Id, command.InitiatorId);

            ArticleRepository.Remove(article);

            EventPublisher.Publish(new ArticleDeleted());
        }
    }
}