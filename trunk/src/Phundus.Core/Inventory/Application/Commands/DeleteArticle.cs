namespace Phundus.Core.Inventory.Application.Commands
{
    using Cqrs;
    using Ddd;
    using Domain.Model.Catalog;
    using IdentityAndAccess.Queries;

    public class DeleteArticle
    {
        public int ArticleId { get; set; }
        public int InitiatorId { get; set; }
    }

    public class DeleteArticleHandler : IHandleCommand<DeleteArticle>
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(DeleteArticle command)
        {
            var article = ArticleRepository.GetById(command.ArticleId);

            MemberInRole.ActiveChief(article.OrganizationId, command.InitiatorId);

            ArticleRepository.Remove(article);

            EventPublisher.Publish(new ArticleDeleted());
        }
    }
}