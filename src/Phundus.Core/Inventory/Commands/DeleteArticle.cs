namespace Phundus.Core.Inventory.Commands
{
    using System.Security;
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using InventoryCtx.Repositories;
    using Model;

    public class DeleteArticle
    {
        public int ArticleId { get; set; }
        public int InitiatorId { get; set; }
    }

    public class DeleteArticleHandler : IHandleCommand<DeleteArticle>
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IMemberInMembershipRoleQueries MemberInMembershipRoleQueries { get; set; }

        public void Handle(DeleteArticle command)
        {
            var article = ArticleRepository.ById(command.ArticleId);
            if (article == null)
                throw new ArticleNotFoundException();

            if (!MemberInMembershipRoleQueries.IsActiveChiefIn(article.OrganizationId, command.InitiatorId))
                throw new SecurityException();

            ArticleRepository.Remove(article);

            EventPublisher.Publish(new ArticleDeleted());
        }
    }
}